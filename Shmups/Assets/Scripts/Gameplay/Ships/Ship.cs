
using UnityEngine;

using Acedia;

#if UNITY_EDITOR
using Handles = UnityEditor.Handles;
#endif

namespace Shmup
{
    public enum Side
    {
        Player,
        Enemy,
        Neutral,
    }

    [DefaultExecutionOrder(ExecutionOrder)]
    public class Ship : MonoBehaviour
    {
        public const int ExecutionOrder = 0;

        #region Input
        [MustInherit(typeof(IShipInput))]
        [SerializeField] Object _shipInput;

        public IShipInput ShipInput => _shipInput == null ? null : (_shipInput as IShipInput);

        [field: SerializeField]
        [field: ShowIf(nameof(HasInput), true)]
        public bool InputEnabled { get; set; } = true;

        public bool HasInput() => ShipInput != null;

        public float LastUpdateTime { get; private set; }
        #endregion

        #region Rig
        [field: SerializeField]
        public Vector2 MuzzleOffset { get; private set; }
        #endregion

        #region States
        [field: SerializeField]
        public Side Side { get; private set; } = Side.Neutral;
        public bool Invincible { get; set; } = false;

        public float SpawnTime { get; private set; }
        #endregion

        #region Events
        public delegate void InputStateDelegate(ShipInputState inputState);
        public delegate void OnShipDelegate(Ship ship);
        
        public event InputStateDelegate OnGetInputState;
        public event OnShipDelegate OnShipDestroyed;
        #endregion

        private void Start()
        {
            if (!InputAvailable()) return;

            SpawnTime = Time.time;
        }

        private void OnEnable()
        {
            LastUpdateTime = Time.time;
        }

        private void Update()
        {
            if (!InputAvailable()) return;
            if (InputEnabled)
            {
                ShipInputState inputState = ShipInput.GetInputState(this);
                OnGetInputState(inputState);
            }

            LastUpdateTime = Time.time;
        }

        private bool InputAvailable()
        {
            if (ShipInput == null)
            {
                //Debug.LogWarning($"Ship named {name} doesn't have an input provider.");
                //enabled = false;
                return false;
            }

            return true;
        }

        public void DestroyShip()
        {
            OnShipDestroyed?.Invoke(this);
        }

        #region EDITOR
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Handles.color = new Color(1f, 0f, 0f, .5f);
            Handles.matrix = transform.localToWorldMatrix;

            Handles.DrawSolidDisc(MuzzleOffset, Vector3.back, .1f);
        }
#endif
        #endregion
    }
}
