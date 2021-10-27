using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text m_DistanceText, m_CoinText, m_BonusTimerText;

    public GameObject m_BonusPanel;

    public bool bonusVisible
    {
        set
        {
            if(value != m_BonusVisible)
            {
                m_BonusVisible = value;
                m_BonusPanel.SetActive(m_BonusVisible);
            }
        }
    }

    private bool m_BonusVisible = false;

    public void ResetAll()
    {
        m_BonusVisible = false;
        m_BonusPanel.SetActive(m_BonusVisible);
    }

    public void SetDistance(float d)
    {
        m_DistanceText.text = string.Format("{0:0.0}", d);
    }

    public void SetCoins(int c)
    {
        m_CoinText.text = c.ToString();
    }

    public void SetBonusTimer(float t)
    {
        m_BonusTimerText.text = string.Format("{0:0.0}", t);
    }
}
