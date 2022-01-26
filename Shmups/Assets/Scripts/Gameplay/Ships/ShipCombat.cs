
using UnityEngine;

namespace Shmup
{
    [RequireComponent(typeof(Ship))]
    [DefaultExecutionOrder(ExecutionOrder)]
    public class ShipCombat : MonoBehaviour
    {
        public const int ExecutionOrder = Ship.ExecutionOrder + 1;

        [field: SerializeField]
        public Weapon Weapon { get; private set; }

        [field: SerializeField]
        public Weapon SecondaryWeapon { get; private set; }

        public float LastShootTime { get; private set; } = -1f;

        private Ship _ship = null;
        public Ship Ship
        {
            private set => _ship = value;
            get
            {
                if (_ship == null) _ship = GetComponent<Ship>();
                return _ship != null ? _ship : throw new MissingComponentException($"{nameof(Ship)} component not found in object named {name}.");
            }
        }

        private void OnEnable()
        {
            Ship.OnGetInputState += HandleInput;
        }

        private void OnDisable()
        {
            Ship.OnGetInputState -= HandleInput;
        }

        private void HandleInput(ShipInputState inputState)
        {
            if (!enabled) return;

            if (inputState.shootPressed) Shoot(false);
            else if (inputState.shoot2Pressed) Shoot(true);
        }

        public void Shoot(bool secondary = false)
        {
            Weapon weapon = secondary ? SecondaryWeapon : Weapon;
            if (weapon.Shoot(this)) LastShootTime = Time.time;
        }

        public void ChangeWeapon(Weapon weapon)
        {
            Weapon = weapon;
            // CONSIDER: OnWeaponChange Event
        }
    }
}
