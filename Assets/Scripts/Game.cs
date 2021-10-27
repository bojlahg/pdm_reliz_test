using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public SpriteRenderer m_CharacterRenderer;
    public Sprite[] m_SkinSprites;

    public Transform m_Floor, m_Player;
    public GameObject m_ObstaclePrefab, m_CoinPrefab, m_SkipBonus, m_JumpBonus;
    public float m_VelocityScale = 1;

    private bool m_GameStarted = false,
        m_JumpPrev = false, // ��������� ������ � ���������� �����
        m_Jump = false; // ��������� ������
    private float m_Velocity = 0,   // ��������
        m_Distance = 0, // ���������� ���������
        m_TargetVelocity = 1,   // ������� ��������
        m_FloorDistance = 0,    // ��������� ���� - ��� ������������
        m_JumpHeight = 1.5f,  // ������ ������ �������
        m_JumpHeightStarted = 1.5f, // ������ ������ � ��������
        m_JumpProgress = 0;

    public void StartGame(int skin)
    {
        m_GameStarted = true;
        m_CharacterRenderer.sprite = m_SkinSprites[skin];
        App.instance.m_HUD.gameObject.SetActive(true);
        App.instance.m_HUD.ResetAll();
    }

    private void Update()
    {
        if(m_GameStarted)
        {
            // ����������
            bool jump = ((Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
            if(jump && !m_Jump)
            {
                m_Jump = jump;
                m_JumpProgress = 0;
                m_JumpHeightStarted = m_JumpHeight;
            }

            // ��������� �������������� �������
            float deltapos = m_VelocityScale * m_Velocity * Time.deltaTime;
            m_Distance += deltapos;
            m_FloorDistance -= deltapos;

            // ������ ������� ��������
            float delta = m_TargetVelocity - m_Velocity;
            if(delta > 0)
            {
                delta = 1;
            }
            else if (delta < 0)
            {
                delta = -1;
            }
            m_Velocity += delta * Time.deltaTime;

            // ������
            if (m_Jump)
            {
                Vector3 plpos = m_Player.localPosition;
                plpos.y = m_JumpHeightStarted * (1 - Mathf.Pow(m_JumpProgress * 2 - 1, 2)); // ��������
                m_Player.localPosition = plpos;
                m_JumpProgress += 0.85f * Time.deltaTime;
                if(m_JumpProgress >= 1)
                {
                    m_Jump = false;
                    m_Player.localPosition = Vector3.zero;
                }
            }

            // ���������� ��� ��� �������� ��������
            if (m_FloorDistance < -16)
            {
                m_FloorDistance += 16.0f;
                m_Floor.localPosition -= Vector3.right * 16.0f;
            }
            Vector3 flpos = m_Floor.localPosition;
            flpos.x = (10 + m_FloorDistance);
            m_Floor.localPosition = flpos;

            // ��������� UI
            App.instance.m_HUD.SetDistance(m_Distance);
        }
    }
}
