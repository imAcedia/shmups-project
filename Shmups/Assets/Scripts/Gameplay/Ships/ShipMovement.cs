
using UnityEngine;

namespace Shmup
{
    [RequireComponent(typeof(Ship))]
    [DefaultExecutionOrder(ExecutionOrder)]
    public class ShipMovement : MonoBehaviour
    {
        public const int ExecutionOrder = Ship.ExecutionOrder + 1;

        /// <summary>The ships move speed in Units per Second</summary>
        [field: SerializeField]
        public float MoveSpeed { get; private set; } = 3f;

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

            if (inputState.movement.sqrMagnitude > 0f)
            {
                Vector3 velocity = inputState.movement;
                velocity *= MoveSpeed;
                velocity *= Time.deltaTime;

                transform.position += velocity;

                Physics2D.SyncTransforms();
                Physics.SyncTransforms();
            }
        }
    }
}
