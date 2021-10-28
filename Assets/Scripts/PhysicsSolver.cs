using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics
{
    [ExecuteInEditMode]
    public class PhysicsSolver : MonoBehaviour
    {
        private void Update()
        {
            PhysicalRect[] phys = GameObject.FindObjectsOfType<PhysicalRect>();
            // Сброс флагов
            for (int i = 0; i < phys.Length; ++i)
            {
                phys[i].ClearCollisionFlags();
            }
            // Проверяем только коллизию с триггерами
            for (int i = 0; i < phys.Length; ++i)
            {
                if (!phys[i].m_Trigger)
                {
                    phys[i].m_Triggered = false;
                    Rect crect = new Rect((Vector2)phys[i].transform.position + phys[i].m_Offset - 0.5f * phys[i].m_Size, phys[i].m_Size);
                    for (int j = 0; j < phys.Length; ++j)
                    {
                        if (phys[j].m_Trigger)
                        {
                            Rect trect = new Rect((Vector2)phys[j].transform.position + phys[j].m_Offset - 0.5f * phys[j].m_Size, phys[j].m_Size);
                            bool clsn = crect.Overlaps(trect);
                            phys[i].m_Triggered = clsn;
                            phys[j].m_Triggered = clsn;

                            if (clsn)
                            {
                                phys[i].TriggerCollision(phys[j]);
                                phys[j].TriggerCollision(phys[i]);
                            }
                        }
                    }
                }
            }
            // Обработка выхода из триггера
            for (int i = 0; i < phys.Length; ++i)
            {
                phys[i].DetectTriggerExit();
            }
        }

        private void OnDrawGizmos()
        {
            // Отладочная отрисовка 
            PhysicalRect[] phys = GameObject.FindObjectsOfType<PhysicalRect>();

            for (int i = 0; i < phys.Length; ++i)
            {
                Gizmos.color = phys[i].m_Triggered ? Color.red : (phys[i].m_Trigger ? Color.yellow : Color.green);
                Gizmos.matrix = phys[i].transform.localToWorldMatrix;
                Gizmos.DrawWireCube(phys[i].m_Offset, phys[i].m_Size);
            }
        }
    }

}