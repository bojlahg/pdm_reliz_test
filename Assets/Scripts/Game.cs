using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public enum BonusType
    {
        None,
        Skip,
        Jump,
        Coin
    }

    public Animator m_CharacterAnimator;
    public SpriteRenderer m_CharacterBodyRenderer;
    public SpriteRenderer[] m_CharacterLegRenderers;

    public Sprite[] m_SkinBodySprites, m_SkinRightLegSprites, m_SkinLeftLegSprites;

    public Transform m_Floor, m_Player, m_Tractor;
    public GameObject[] m_ObstaclePrefabs, m_BonusPrefabs;
    public float m_VelocityScale = 1;

    private bool m_GameStarted = false,
        m_JumpPrev = false, // состояние прыжка в предыдущем кадре
        m_Jump = false; // состояние прыжка
    private float m_Velocity = 0,   // Скорость
        m_DeltaPos = 0, // Пройденная дистанция за кадр
        m_Distance = 0, // Пройденная дистанция
        m_TargetVelocity = 1,   // Целевая скорость
        m_BonusJumpHeight = 3.0f,  // Высота прыжка текущая
        m_UsualJumpHeight = 1.5f,  // Высота прыжка обычная
        m_JumpHeight = 1.5f,  // Высота прыжка текущая
        m_JumpHeightStarted = 1.5f, // Высота прыжка в процессе
        m_JumpProgress = 0, // Прогресс анимации прыжка
        m_SpawnDistance = 0, // Дистанция проденная до следующего спавна
        m_BonusDuration = 30, // Продолжительность бонуса
        m_BonusTimer = 0,  // Таймер бонуса
        m_BonusProbability = 0.25f; // Вероятность появления бонуса

    private int m_CoinCount = 0; // Кол-во собранных монет

    private BonusType m_ActiveBonus = BonusType.None;

    public void StartGame(int skin)
    {
        m_GameStarted = true;
        m_CharacterBodyRenderer.sprite = m_SkinBodySprites[skin];
        m_CharacterLegRenderers[0].sprite = m_SkinRightLegSprites[skin];
        m_CharacterLegRenderers[1].sprite = m_SkinLeftLegSprites[skin];
        App.instance.m_HUD.gameObject.SetActive(true);
        App.instance.m_HUD.ResetAll();
    }

    public void BonusTaken(BonusType bt)
    {
        // Был взят бонус
        m_ActiveBonus = bt;
        m_BonusTimer = m_BonusDuration;

        switch(m_ActiveBonus)
        {
            case BonusType.Coin:
                ++m_CoinCount;
                App.instance.m_HUD.SetCoins(m_CoinCount);
                break;

            case BonusType.Jump:
                SoundManager.instance.PlayLooped("BonusMusicLoop");
                App.instance.m_HUD.bonusVisible = true;
                App.instance.m_HUD.SetBonusTimer(m_BonusTimer);
                m_JumpHeight = m_BonusJumpHeight;
                break;

            case BonusType.Skip:
                SoundManager.instance.PlayLooped("BonusMusicLoop");
                App.instance.m_HUD.bonusVisible = true;
                App.instance.m_HUD.SetBonusTimer(m_BonusTimer);
                break;
        }
        SoundManager.instance.PlayOnce("Bonus");
    }

    public bool SkipObstacle()
    {
        // Нужно ли игнорировать препятствие
        return m_ActiveBonus == BonusType.Skip;
    }

    private void Update()
    {
        if (m_GameStarted)
        {
            UpdateVelocity();
            UpdateJump();
            UpdateSpawner();
            UpdateTractor();
            UpdatePassed();
            UpdateFloor();

            // Обновляем UI
            App.instance.m_HUD.SetDistance(m_Distance);
        }
    }

    private void UpdateVelocity()
    {
        // Изменение горизонтальной позиции
        m_DeltaPos = m_VelocityScale * m_Velocity * Time.deltaTime;
        m_Distance += m_DeltaPos;

        // Расчет текущей скорости
        float delta = m_TargetVelocity - m_Velocity;
        if (delta > 0)
        {
            delta = 1;
        }
        else if (delta < 0)
        {
            delta = -1;
        }
        m_Velocity += delta * Time.deltaTime;

        m_CharacterAnimator.SetFloat("Velocity", m_Velocity);
    }

    private void UpdateJump()
    {
        // Управление
        bool jump = ((Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        if (jump && !m_Jump)
        {
            SoundManager.instance.PlayOnce("Jump");
            m_Jump = jump;
            m_JumpProgress = 0;
            m_JumpHeightStarted = m_JumpHeight;
            m_CharacterAnimator.SetBool("Jump", true);
            m_CharacterAnimator.SetFloat("JumpTime", 0);
        }
        // Прыжок
        if (m_Jump)
        {
            Vector3 plpos = m_Player.localPosition;
            plpos.y = m_JumpHeightStarted * (1 - Mathf.Pow(m_JumpProgress * 2 - 1, 2)); // Парабола
            m_Player.localPosition = plpos;
            m_JumpProgress += 0.85f * Time.deltaTime;
            if (m_JumpProgress >= 1)
            {
                m_Jump = false;
                m_CharacterAnimator.SetBool("Jump", false);
                m_CharacterAnimator.SetFloat("JumpTime", 1);
                m_Player.localPosition = Vector3.zero;
            }
            else
            {
                m_CharacterAnimator.SetFloat("JumpTime", m_JumpProgress);
            }
        }
    }

    private void UpdateSpawner()
    {
        // Спавн препятствий и бонусов
        if (m_ActiveBonus != BonusType.None)
        {
            m_BonusTimer -= Time.deltaTime;

            if(m_ActiveBonus == BonusType.Skip || m_ActiveBonus == BonusType.Jump)
            {
                App.instance.m_HUD.SetBonusTimer(m_BonusTimer);
            }

            if(m_BonusTimer < 0)
            {
                // Бонус кончился
                SoundManager.instance.StopLooped("BonusMusicLoop");
                App.instance.m_HUD.bonusVisible = false;
                m_ActiveBonus = BonusType.None;

                m_JumpHeight = m_UsualJumpHeight;
            }
        }
        m_SpawnDistance -= m_DeltaPos;
        if (m_SpawnDistance <= 0)
        {
            m_SpawnDistance = 15 + Random.Range(0, 10);
            GameObject newgo;
            if (m_ActiveBonus == BonusType.None && Random.value <= m_BonusProbability)
            {
                // Бонус
                newgo = GameObject.Instantiate(m_BonusPrefabs.GetRandom<GameObject>(), Vector3.right * 20 + Vector3.up * Random.Range(1.0f, 2.0f), Quaternion.identity);
                Bonus bonus = newgo.GetComponent<Bonus>();
                bonus.m_Game = this;
            }
            else
            {
                // Препятствие
                newgo = GameObject.Instantiate(m_ObstaclePrefabs.GetRandom<GameObject>(), Vector3.right * 20, Quaternion.identity);
                Obstacle obstacle = newgo.GetComponent<Obstacle>();
                obstacle.m_Game = this;
            }
            newgo.transform.SetParent(m_Tractor);
        }
    }

    private void UpdateTractor()
    {
        // Перемещаем слой объектов для имитации движения
        Vector3 trpos = m_Tractor.localPosition;
        trpos.x -= m_DeltaPos;
        if (trpos.x < 20)
        {
            // Перемещаем обратно к началу координат
            trpos.x += 20;
            for (int i = 0; i < m_Tractor.childCount; ++i)
            {
                Transform child = m_Tractor.GetChild(i);
                Vector3 chpos = child.localPosition;
                chpos.x -= 20;
                child.localPosition = chpos;
            }
        }
        m_Tractor.localPosition = trpos;
    }

    private void UpdatePassed()
    {
        // Удаляем уехавшие далеко объекты
        for (int i = 0; i < m_Tractor.childCount; ++i)
        {
            Transform child = m_Tractor.GetChild(i);
            if (child.position.x < -20)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    private void UpdateFloor()
    {
        // Перемещаем пол для создания видимости движения
        Vector3 flpos = m_Floor.localPosition;
        flpos.x -= m_DeltaPos;
        if(flpos.x < 0)
        {
            flpos.x += 10;
        }
        m_Floor.localPosition = flpos;
    }
}
