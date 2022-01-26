using UnityEngine;

using Action = System.Action;

namespace Shmup
{
    [CreateAssetMenu(menuName = "Input/Ship/Keyboard", order = AssetMenu.order)]
    public class ShipKeyboardInput : ScriptableObject, IShipInput
    {
        public bool normalizeMovement = true;

        public Keys primaryKeys = new Keys()
        {
            shoot = KeyCode.Z,
            shoot2 = KeyCode.X,

            up = KeyCode.UpArrow,
            down = KeyCode.DownArrow,
            left = KeyCode.LeftArrow,
            right = KeyCode.RightArrow,
        };

        public Keys secondaryKeys = new Keys()
        {
            shoot = KeyCode.RightControl,
            shoot2 = KeyCode.RightShift,

            up = KeyCode.W,
            down = KeyCode.S,
            left = KeyCode.A,
            right = KeyCode.D,
        };

        public event Action OnShootDown;
        public event Action OnBombDown;

        public ShipInputState GetInputState(Ship ship)
        {
            ShipInputState inputState = new ShipInputState();

            inputState.movement = GetMovement(primaryKeys) + GetMovement(secondaryKeys);
            if (normalizeMovement) inputState.movement.Normalize();

            inputState.shootPressed = Input.GetKey(primaryKeys.shoot) || Input.GetKey(secondaryKeys.shoot);
            inputState.shoot2Pressed = Input.GetKey(primaryKeys.shoot2) || Input.GetKey(secondaryKeys.shoot2);

            return inputState; // END

            static Vector2 GetMovement(Keys keys)
            {
                Vector2 movement = new Vector2();

                if (Input.GetKey(keys.up)) movement.y += 1f;
                if (Input.GetKey(keys.left)) movement.x -= 1f;
                if (Input.GetKey(keys.down)) movement.y -= 1f;
                if (Input.GetKey(keys.right)) movement.x += 1f;

                return movement;
            }
        }

        [System.Serializable]
        public struct Keys
        {
            public KeyCode shoot;
            public KeyCode shoot2;

            [Space]
            public KeyCode up;
            public KeyCode down;
            public KeyCode left;
            public KeyCode right;
        }
    }
}
