using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using Handles = UnityEditor.Handles;
#endif

namespace Shmup
{
    [RequireComponent(typeof(Ship))]
    [DefaultExecutionOrder(ExecutionOrder)]
    public class PlayerBounding : MonoBehaviour
    {
        public const int ExecutionOrder = ShipMovement.ExecutionOrder + 10;

        public Rect _bounds = new Rect()
        {
            size = new Vector2(7f * 3.2f, 32f),
            position = new Vector2(0f, 8f),
        };

        public Rect Bounds => new Rect()
        {
            size = _bounds.size,
            center = _bounds.position,
        };

        [field: SerializeField]
        public Collider2D Collider { get; private set; }

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
                BoundPlayer();
        }

        public void BoundPlayer()
        {
            Bounds colliderBounds = Collider.bounds;
            Vector3 position = transform.position;

            float diff = Bounds.xMin - colliderBounds.min.x;
            if (diff > 0f) position.x += diff;

            diff = Bounds.yMin - colliderBounds.min.y;
            if (diff > 0f) position.y += diff;

            diff = Bounds.xMax - colliderBounds.max.x;
            if (diff < 0f) position.x += diff;

            diff = Bounds.yMax - colliderBounds.max.y;
            if (diff < 0f) position.y += diff;

            transform.position = position;

            Physics2D.SyncTransforms();
            Physics.SyncTransforms();
        }

#if UNITY_EDITOR
        protected void OnDrawGizmosSelected()
        {
            Rect rect = Bounds;

            Handles.color = new Color(0f, 1f, 0f, .1f);
            Handles.DrawSolidRectangleWithOutline(rect, Color.white, Color.white);
        }
#endif
    }
}
