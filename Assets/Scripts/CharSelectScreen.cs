using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelectScreen : MonoBehaviour
{
    

    public void Awake()
    {
        App.instance.m_Game.gameObject.SetActive(false);
        App.instance.m_HUD.gameObject.SetActive(false);
    }

    public void Character1_ButtonClick()
    {
        App.instance.m_CharScr.gameObject.SetActive(false);
        App.instance.m_Game.gameObject.SetActive(true);
        App.instance.m_Game.StartGame(0);
    }

    public void Character2_ButtonClick()
    {
        App.instance.m_CharScr.gameObject.SetActive(false);
        App.instance.m_Game.gameObject.SetActive(true);
        App.instance.m_Game.StartGame(1);
    }
}
