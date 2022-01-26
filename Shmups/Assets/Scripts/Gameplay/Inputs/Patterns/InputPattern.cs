
using System;
using UnityEngine;

namespace Shmup
{
    public abstract class InputPattern : ScriptableObject
    {
        public abstract ShipInputState GetInputState(Ship ship);

        public abstract int ShouldTriggerShoot(Ship ship);
        public abstract int ShouldTriggerBomb(Ship ship);
    }
}
