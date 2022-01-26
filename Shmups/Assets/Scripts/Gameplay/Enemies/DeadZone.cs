using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shmup
{
    public class DeadZone : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Collider2D collider = collision.collider;

            if (collider.TryGetComponent(out Ship ship))
                Destroy(ship.gameObject);

            else if (collider.TryGetComponent(out BasicBullet bullet))
                bullet.Despawn();
        }
    }
}
