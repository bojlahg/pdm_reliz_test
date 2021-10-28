using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics
{
    public interface ITriggerInterface
    {
        void OnCustomTriggerEnter(CustomPhysics.PhysicalRect pr);
        void OnCustomTriggerStay(CustomPhysics.PhysicalRect pr);
        void OnCustomTriggerExit(CustomPhysics.PhysicalRect pr);
    }
}