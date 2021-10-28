using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, CustomPhysics.ITriggerInterface
{
    [HideInInspector]
    public Game m_Game;
    public Sprite m_BrokenSprite;

    private SpriteRenderer m_SpriteRenderer;

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnCustomTriggerEnter(CustomPhysics.PhysicalRect pr)
    {
        if (pr.gameObject.name == "Player" && !m_Game.SkipObstacle())
        {
            m_SpriteRenderer.sprite = m_BrokenSprite;
            SoundManager.instance.PlayOnce("Hit");
        }
    }

    public void OnCustomTriggerExit(CustomPhysics.PhysicalRect pr)
    {

    }

    public void OnCustomTriggerStay(CustomPhysics.PhysicalRect pr)
    {

    }

}
