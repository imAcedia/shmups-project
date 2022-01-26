using UnityEngine;

namespace Shmup
{
    public abstract class SpawnArea : MonoBehaviour
    {
        public abstract Vector2 GetRandomPoint();

#if UNITY_EDITOR
        protected abstract void OnDrawGizmosSelected();
#endif
    }
}