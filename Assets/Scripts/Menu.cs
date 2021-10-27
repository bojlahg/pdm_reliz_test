using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public Game m_Game;
    public HUD m_HUD;

    public void Awake()
    {
        m_Game.gameObject.SetActive(false);
        m_HUD.gameObject.SetActive(false);
    }

    public void Character1_ButtonClick()
    {
        gameObject.SetActive(false);
        m_Game.gameObject.SetActive(true);
        m_Game.StartGame(0);
    }

    public void Character2_ButtonClick()
    {
        gameObject.SetActive(false);
        m_Game.gameObject.SetActive(true);
        m_Game.StartGame(1);
    }
}
