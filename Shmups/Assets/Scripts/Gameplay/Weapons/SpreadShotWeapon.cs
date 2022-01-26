
using UnityEngine;

namespace Shmup
{
    [CreateAssetMenu(menuName = "Weapon/Spread Shot", order = AssetMenu.order)]
    public class SpreadShotWeapon : Weapon
    {
        [field: SerializeField]
        public BasicBullet BulletPrefab { get; private set; }

        /// <summary> The spread angle of the shot in degrees</summary>
        [field: SerializeField]
        public float SpreadAngle { get; private set; } = 90f;

        /// <summary> The number of bullets to shot from a single fire</summary>
        [field: SerializeField]
        public int SpreadCount { get; private set; } = 5;

        /// <summary>The weapons shooting frequency in Bullets per Second</summary>
        [field: SerializeField]
        public float ShootFrequency { get; private set; } = 1f;

        /// <summary>The time between each shot in seconds</summary>
        public float ShootPeriod => 1f / ShootFrequency;

        public override bool Shoot(ShipCombat shipCombat)
        {
            // if still in cooldown, then don't do anything
            if (Time.time < shipCombat.LastShootTime + ShootPeriod) return false;

            Transform shipTransform = shipCombat.transform;
            Ship ship = shipCombat.Ship;

            Vector3 spawnPosition = shipTransform.TransformPoint(ship.MuzzleOffset);

            BasicBulletPool.prefabPools.TryGetValue(BulletPrefab, out BasicBulletPool pool);
            for (int i = 0; i < SpreadCount; i++)
            {
                float a = shipTransform.eulerAngles.z;
                a += (i + .5f) * SpreadAngle / SpreadCount;
                a -= SpreadAngle * .5f;

                Quaternion rotation = Quaternion.Euler(0f, 0f, a);

                BasicBullet bullet;
                if (pool == null)
                    bullet = Instantiate(BulletPrefab, spawnPosition, rotation);
                
                else bullet = pool.Spawn(spawnPosition, rotation);

                bullet.Initialize(shipCombat, pool);
            }

            return true;
        }
    }
}
