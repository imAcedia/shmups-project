using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Acedia;

namespace Shmup
{
    public class BasicBullet : MonoBehaviour
    {
        public float moveSpeed = 10f;

        public ShipCombat Owner { get; private set; }
        public BasicBulletPool Pool { get; private set; }
        public bool Initialized { get; private set; } = false;

        [field: SerializeField]
        public Renderer Graphics { get; private set; }

        public void Initialize(ShipCombat owner, BasicBulletPool pool)
        {
            Owner = owner;
            Pool = pool;

            Initialized = true;

            //VVVVVVVVVV TEMP VVVVVVVVVVV

            StartCoroutine(Temp());
            IEnumerator Temp()
            {
                yield return new WaitForSeconds(1f);
                Destroy(gameObject);
            }
        }

        public void Despawn()
        {
            if (Pool == null) Destroy(gameObject);
            
            else Pool.Despawn(this);
        }

        private void Update()
        {
            transform.position += moveSpeed * Time.deltaTime * transform.up;
            if (!Graphics.isVisible) Despawn();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Collider2D collider = collision.collider;

            if (!collider.TryGetComponent(out Ship ship)) return;

            if (!ship.enabled) return;
            if (ship.Invincible) return;

            if (Owner != null)
            {
                if (ship == Owner) return;
                if (ship.Side == Owner.Ship.Side) return;
            }

            ship.DestroyShip();
            Despawn();
        }
    }
}
