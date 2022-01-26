using UnityEngine;

#if UNITY_EDITOR
using Handles = UnityEditor.Handles;
#endif

namespace Shmup
{
    public class SquareSpawnArea : SpawnArea
    {
        public override Vector2 GetRandomPoint()
        {
            Vector2 point = new Vector2()
            {
                x = Random.Range(-0.5f, 0.5f),
                y = Random.Range(-0.5f, 0.5f),
            };

            return transform.TransformPoint(point);
        }

#if UNITY_EDITOR
        protected override void OnDrawGizmosSelected()
        {
            Rect rect = new Rect()
            {
                size = Vector3.one,
                center = Vector2.zero,
            };

            Handles.color = new Color(1f, 0f, 0f, .1f);
            Handles.matrix = transform.localToWorldMatrix;
            Handles.DrawSolidRectangleWithOutline(rect, Color.white, Color.white);
        }
#endif
    }
}