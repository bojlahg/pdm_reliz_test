using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour, CustomPhysics.ITriggerInterface
{
    [HideInInspector]
    public Game m_Game;
    public Game.BonusType m_BonusType;

    public void OnCustomTriggerEnter(CustomPhysics.PhysicalRect pr)
    {
        SoundManager.instance.PlayOnce("Bonus");
        m_Game.BonusTaken(m_BonusType);

        GameObject.Destroy(gameObject);
    }

    public void OnCustomTriggerExit(CustomPhysics.PhysicalRect pr)
    {

    }

    public void OnCustomTriggerStay(CustomPhysics.PhysicalRect pr)
    {

    }
}
