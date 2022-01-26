using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace Shmup
{
    [RequireComponent(typeof(Ship))]
    [DefaultExecutionOrder(ExecutionOrder)]
    public class ShipMessager : MonoBehaviour
    {
        public const int ExecutionOrder = Ship.ExecutionOrder - 1;
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

        public UnityEvent<Ship> onShipDestroyed;
        public UnityEvent<ShipInputState> onGetInputState;

        private void Awake()
        {
            Ship.OnShipDestroyed += onShipDestroyed.Invoke;
            Ship.OnGetInputState += onGetInputState.Invoke;
        }
    }
}
