using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shmup
{
    public class BasicBulletPool : MonoBehaviour
    {
        public static Dictionary<BasicBullet, BasicBulletPool> prefabPools = new Dictionary<BasicBullet, BasicBulletPool>();

        public BasicBullet prefab;

        public int preloadCount = 50;

        public HashSet<BasicBullet> pool = new HashSet<BasicBullet>();
        public Stack<BasicBullet> inactives = new Stack<BasicBullet>();

        private void Start()
        {
            if (prefabPools.ContainsKey(prefab) && prefabPools[prefab] != null)
            {
                Debug.LogError("Multiple pool exists.");
                gameObject.SetActive(false);
                return;
            }

            prefabPools[prefab] = this;
            PreLoad();
        }

        public void PreLoad()
        {
            for (int i = 0; i < preloadCount; i++)
            {
                BasicBullet bullet = CreateNew();
                bullet.gameObject.SetActive(false);
                inactives.Push(bullet);
            }
        }

        public BasicBullet Spawn(Vector3 position, Quaternion rotation)
        {
            if (!inactives.TryPop(out BasicBullet bullet))
                bullet = CreateNew();

            bullet.transform.position = position;
            bullet.transform.rotation = rotation;
            bullet.gameObject.SetActive(true);

            return bullet;
        }

        public void Despawn(BasicBullet bullet)
        {
            if (!pool.Contains(bullet)) throw new System.Exception($"{bullet.name} doesn't belong in the pool.");

            bullet.gameObject.SetActive(false);
            inactives.Push(bullet);
        }

        private BasicBullet CreateNew()
        {
            BasicBullet bullet = Instantiate(prefab, transform);
            pool.Add(bullet);
            return bullet;
        }
    }
}
