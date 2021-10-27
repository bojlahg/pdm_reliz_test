using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics
{
    public class PhysicalRect : MonoBehaviour
    {
        internal class PhysicalCollision
        {
            public bool m_Collided;
            public PhysicalRect m_PhysicalRect;
        }

        public Vector2 m_Offset = Vector2.zero, m_Size = new Vector2(1, 1);
        public bool m_Trigger = false;

        internal bool m_Triggered = false;
        internal List<PhysicalCollision> m_Collisions = new List<PhysicalCollision>();

        internal void ClearCollisionFlags()
        {
            // Сброс флагов для детектора выхода
            for (int i = 0; i < m_Collisions.Count; ++i)
            {
                m_Collisions[i].m_Collided = false;
            }
        }

        internal void TriggerCollision(PhysicalRect pr)
        {
            // Обработка коллизии
            int foundIdx = -1;
            for(int i = 0; i < m_Collisions.Count; ++i)
            {
                if(m_Collisions[i].m_PhysicalRect == pr)
                {
                    foundIdx = i;
                    break;
                }
            }

            if(foundIdx == -1)
            {
                // новая
                PhysicalCollision clsn = new PhysicalCollision();
                clsn.m_Collided = true;
                clsn.m_PhysicalRect = pr;
                m_Collisions.Add(clsn);
                TriggerEnter(pr);
            }
            else
            {
                // уже была
                m_Collisions[foundIdx].m_Collided = true;
                TriggerStay(pr);
            }
        }

        internal void DetectTriggerExit()
        {
            // Определяем выход из триггера
            int cnt = 0;
            for (int i = 0; i < m_Collisions.Count; ++i)
            {
                if (!m_Collisions[i].m_Collided)
                {
                    TriggerExit(m_Collisions[i].m_PhysicalRect);
                    m_Collisions[i] = null;
                    ++cnt;
                }
            }
            m_Collisions.RemoveAll((e)=>(e == null));
        }

        private void TriggerEnter(PhysicalRect pr)
        {
            //Debug.Log("OnTriggerEnter");
        }

        private void TriggerStay(PhysicalRect pr)
        {
            //Debug.Log("OnTriggerStay");
        }

        private void TriggerExit(PhysicalRect pr)
        {
            //Debug.Log("OnTriggerExit");
        }
    }
}
