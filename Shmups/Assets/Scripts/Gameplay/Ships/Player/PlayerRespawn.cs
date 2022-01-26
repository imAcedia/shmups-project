using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shmup
{
    [RequireComponent(typeof(Ship), typeof(Animator))]
    [DefaultExecutionOrder(ExecutionOrder)]
    public class PlayerRespawn : MonoBehaviour
    {
        public const int ExecutionOrder = Ship.ExecutionOrder + 1;

        public float respawnDelay = 2f;
        public float invincibleDuration = 2f;

        //[SerializeField] List<Behaviour> disabledBehaviours = new List<Behaviour>();

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

        private Animator _animator = null;
        public Animator Animator
        {
            private set => _animator = value;
            get
            {
                if (_animator == null) _animator = GetComponent<Animator>();
                return _animator != null ? _animator : throw new MissingComponentException($"{nameof(Ship)} component not found in object named {name}.");
            }
        }

        [field: SerializeField]
        public GameObject Graphics { get; private set; }

        private bool respawning = false;

        private void OnEnable()
        {
            Ship.OnShipDestroyed += ShipDestroyed;
        }

        private void ShipDestroyed(Ship ship)
        {
            if (!respawning) Respawn();
        }

        public void Respawn()
        {
            StartCoroutine(Respawn());
            return; // END

            IEnumerator Respawn()
            {
                respawning = true;

                Ship.Invincible = true;
                Ship.enabled = false;
                Graphics.SetActive(false);

                yield return new WaitForSeconds(respawnDelay);
                transform.position = new Vector3(0f, -14f, 0f);
                Animator.Play("SpawnIn");
                yield return null;

                Ship.enabled = true;
                Graphics.SetActive(true);
                respawning = false;

                yield return new WaitForSeconds(invincibleDuration);

                Ship.Invincible = false;
            }
        }
    }
}
