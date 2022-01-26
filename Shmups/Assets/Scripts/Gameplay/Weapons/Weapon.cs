
using UnityEngine;

namespace Shmup
{
    public abstract class Weapon : ScriptableObject
    {
        public abstract bool Shoot(ShipCombat shipCombat);
    }
}
