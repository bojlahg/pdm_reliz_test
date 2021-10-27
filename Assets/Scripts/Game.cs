using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public HUD m_HUD;
    public SpriteRenderer m_CharacterRenderer;
    public Sprite[] m_SkinSprites;

    public Transform m_Floor;
    
    public float m_VelocityScale = 1;


    private bool m_GameStarted = false;
    private float m_Velocity = 0, m_Distance = 0, m_TargetVelocity = 1, m_FloorDistance = 0;

    public void StartGame(int skin)
    {
        m_GameStarted = true;
        m_CharacterRenderer.sprite = m_SkinSprites[skin];
        m_HUD.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(m_GameStarted)
        {
            // Изменение позиции
            float deltapos = m_VelocityScale * m_Velocity * Time.deltaTime;
            m_Distance += deltapos;
            m_FloorDistance -= deltapos;

            // Расчет текущей скорости
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

            if(m_FloorDistance < -16)
            {
                m_FloorDistance += 16.0f;
                m_Floor.localPosition -= Vector3.right * 16.0f;
            }
            Vector3 flpos = m_Floor.localPosition;
            flpos.x = (10 + m_FloorDistance);
            m_Floor.localPosition = flpos;
        }
    }
}
