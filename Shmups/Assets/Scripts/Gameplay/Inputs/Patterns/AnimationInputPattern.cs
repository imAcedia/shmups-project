using UnityEngine;

namespace Shmup
{
    [CreateAssetMenu(menuName = "Input Pattern/Input Pattern", order = AssetMenu.order)]
    public class AnimationInputPattern : InputPattern
    {
        [field: SerializeField]
        [field: Tooltip("The x movement input value at a given time since the enemy spawned")]
        public AnimationCurve InputCurveX { get; private set; }

        [field: SerializeField]
        [field: Tooltip("The y movement input value at a given time since the enemy spawned")]
        public AnimationCurve InputCurveY { get; private set; }

        [field: SerializeField]
        public InputTrigger[] InputTriggers { get; private set; } = new InputTrigger[0];

        public override ShipInputState GetInputState(Ship ship)
        {
            float timeSinceSpawn = Time.time - ship.SpawnTime;

            ShipInputState inputState = new ShipInputState();

            float x = InputCurveX.Evaluate(timeSinceSpawn);
            float y = InputCurveY.Evaluate(timeSinceSpawn);

            inputState.movement = new Vector2(x, y);

            float time = Time.time;
            foreach (InputTrigger trigger in InputTriggers)
            {
                if (CheckTrigger(time, trigger))
                {
                    inputState.shootPressed = trigger.type is InputTriggerType.Shoot;
                    inputState.shoot2Pressed = trigger.type is InputTriggerType.Bomb;
                }
            }

            return inputState; // END
        }

        public override int ShouldTriggerShoot(Ship ship)
        {
            int triggerCount = 0;

            float time = Time.time;
            float lastUpdateTime = ship.LastUpdateTime;
            foreach (InputTrigger trigger in InputTriggers)
            {
                if (trigger.type is not InputTriggerType.Shoot) continue;
                if (CheckTriggerDown(time, lastUpdateTime, trigger)) triggerCount++;
            }

            return triggerCount;
        }

        public override int ShouldTriggerBomb(Ship ship)
        {
            int triggerCount = 0;

            float time = Time.time;
            float lastUpdateTime = ship.LastUpdateTime;
            foreach (InputTrigger trigger in InputTriggers)
            {
                if (trigger.type is not InputTriggerType.Bomb) continue;
                if (CheckTriggerDown(time, lastUpdateTime, trigger)) triggerCount++;
            }

            return triggerCount;
        }

        private static bool CheckTrigger(float time, InputTrigger trigger)
        {
            return time >= trigger.startTime && time < trigger.endTime;
        }
        private static bool CheckTriggerDown(float time, float lastInputTime, InputTrigger trigger)
        {
            return lastInputTime < trigger.startTime && time >= trigger.startTime;
        }

        #region Types
        [System.Serializable]
        public struct InputTrigger
        {
            public InputTriggerType type;

            public float startTime;
            public float endTime;
        }

        public enum InputTriggerType { Shoot, Bomb }
        #endregion
    }
}
