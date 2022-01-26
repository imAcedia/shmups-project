using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shmup
{
    [RequireComponent(typeof(Ship), typeof(ShipCombat), typeof(Animator))]
    [DefaultExecutionOrder(ExecutionOrder)]
    public class SimpleEnemy : MonoBehaviour
    {
        public const int ExecutionOrder = Ship.ExecutionOrder;

        #region Components
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

        private ShipCombat _shipCombat = null;
        public ShipCombat ShipCombat
        {
            private set => _shipCombat = value;
            get
            {
                if (_shipCombat == null) _shipCombat = GetComponent<ShipCombat>();
                return _shipCombat != null ? _shipCombat : throw new MissingComponentException($"{nameof(Ship)} component not found in object named {name}.");
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
        public Renderer Renderer { get; private set; }
        #endregion

        #region States
        [field: SerializeField]
        public bool IsShooting { get; set; } = false;

        public void ToggleShoot() => IsShooting = !IsShooting;
        #endregion

        public float startDelay = .25f;

        private IEnumerator Start()
        {
            if (startDelay > 0f)
            {
                Animator.speed = 0f;
                yield return new WaitForSeconds(startDelay);
                Animator.speed = 1f;
            }
        }

        private void OnEnable()
        {
            Ship.OnShipDestroyed += ShipDestroyed;
        }

        private void OnDisable()
        {
            Ship.OnShipDestroyed -= ShipDestroyed;
        }

        private float deadTimer = 0f;
        private void Update()
        {
            if (Renderer.isVisible)
            {
                if (IsShooting) ShipCombat.Shoot();
                deadTimer = 0f;
            }
            else
            {
                deadTimer += Time.deltaTime;
                if (deadTimer >= 5f) Destroy(this);
            }
        }

        public void SetupShip(SimpleEnemyData enemyData)
        {
            Animator.runtimeAnimatorController = enemyData.controller;
            ShipCombat.ChangeWeapon(enemyData.weapon);
        }

        private void ShipDestroyed(Ship ship)
        {
            Destroy(ship.gameObject);
        }
    }
}
