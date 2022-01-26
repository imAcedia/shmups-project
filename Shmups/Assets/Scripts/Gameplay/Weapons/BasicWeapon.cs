
using UnityEngine;

namespace Shmup
{
    [CreateAssetMenu(menuName = "Weapon/Basic", order = AssetMenu.order)]
    public class BasicWeapon : Weapon
    {
        [field: SerializeField]
        public BasicBullet BulletPrefab { get; private set; }

        /// <summary>The weapons shooting frequency in Bullets per Second</summary>
        [field: SerializeField]
        public float ShootFrequency { get; private set; } = 3f;

        // TODO: Optimize division?
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
            BasicBullet bullet;

            if (pool == null)
                bullet = Instantiate(BulletPrefab, spawnPosition, shipTransform.rotation);

            else
                bullet = pool.Spawn(spawnPosition, shipTransform.rotation);

            bullet.Initialize(shipCombat, pool);
            return true;
        }
    }
}
