using UnityEngine;

using Action = System.Action;

namespace Shmup
{
    public class ShipEnemyPatternInput : ScriptableObject, IShipInput
    {
        public InputPattern enemyPattern;

        public event Action OnShootDown;
        public event Action OnBombDown;

        public ShipInputState GetInputState(Ship ship)
        {
            for (int i = 0; i < enemyPattern.ShouldTriggerShoot(ship); i++)
                OnShootDown();

            for (int i = 0; i < enemyPattern.ShouldTriggerBomb(ship); i++)
                OnBombDown();

            return enemyPattern.GetInputState(ship);
        }
    }
}
