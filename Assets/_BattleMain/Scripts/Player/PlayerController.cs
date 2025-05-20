using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleArena
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private MeshRenderer _renderer;

        [SerializeField] private GameObject splashParticleObj;
        [SerializeField] private GameObject bloodParticleObj;

        [SerializeField] private float maxHealth;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotateSpeed = 100f;

        public Action<float, float> OnHealthUpdate;

        private float _currentHealth;
        private WeaponController _weaponController;
        private GameController _gameController;
        private CharacterController _characterController;
        private bool isDead;

        private void Awake()
        {
            _weaponController = GetComponent<WeaponController>();
            _characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            SetHealth(maxHealth);
            StartCoroutine(Engage());
        }

        public void Init(GameController gc)
        {
            _gameController = gc;
        }

        IEnumerator Engage()
        {
            while (!isDead)
            {
                var target = _gameController.GetRandomOtherPlayer(this);
                if (!target) break;

                yield return MoveInRange(target.transform, _weaponController.CurrentWeapon.WeaponDetails.Range);
                if (!target) break;
                yield return new WaitForSeconds(_weaponController.CurrentWeapon.WeaponDetails.AttackSpeed);
                if (target) _weaponController.ShootAtTarget(target);
            }
        }

        IEnumerator MoveInRange(Transform targetTransform, float range)
        {
            while (true)
            {
                try
                {
                    var direction = (targetTransform.position - transform.position).normalized;
                    var targetRotation = Quaternion.LookRotation(direction, Vector3.up);

                    var distance = Vector3.Distance(transform.position, targetTransform.position);
                    if (distance > range)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, targetTransform.position,
                            moveSpeed * Time.deltaTime);
                    }

                    var angle = Quaternion.Angle(transform.rotation, targetRotation);
                    if (angle > 0.5f)
                    {
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                            rotateSpeed * Time.deltaTime);
                    }

                    if (distance <= range && angle <= 0.5f) break;
                }
                catch (Exception e)
                {
                    break;
                }

                yield return null;
            }
        }

        private void SetHealth(float h)
        {
            _currentHealth = h;
            OnHealthUpdate?.Invoke(_currentHealth, maxHealth);
        }

        public void SetColor(Color playerColor)
        {
            var materialProperty = new MaterialPropertyBlock();
            materialProperty.SetColor("_BaseColor", playerColor);
            _renderer.SetPropertyBlock(materialProperty);
        }

        public void Hit(float damage)
        {
            var newHealth = _currentHealth - damage;
            if (newHealth <= 0)
            {
                newHealth = 0;
                Die();
            }
            else
            {
                var impactObj = Instantiate(bloodParticleObj, transform.position, Quaternion.identity);
            }

            SetHealth(newHealth);
        }

        private void Die()
        {
            StopAllCoroutines();
            isDead = true;
            _gameController.PlayerDead(this);
            var splashObj = Instantiate(splashParticleObj, transform.position, Quaternion.identity);
            // Destroy(splashObj, 1f);
            Destroy(gameObject);
        }

        public void SetWeapon(Weapon weapon)
        {
            _weaponController.SetWeapon(weapon);
        }
    }
}