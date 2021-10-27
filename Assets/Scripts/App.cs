using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour
{
    public Game m_Game;
    public HUD m_HUD;
    public CharSelectScreen m_CharScr;

    public static App instance { get { return m_Instance; } }

    private static App m_Instance;

    private void Awake()
    {
        m_Instance = this;
    }

    private void Start()
    {
        m_Game.gameObject.SetActive(false);
        m_HUD.gameObject.SetActive(false);
        m_CharScr.gameObject.SetActive(true);
    }
}
