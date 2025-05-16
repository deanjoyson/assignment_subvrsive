using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleArena
{
    public class Bullet : MonoBehaviour
    {
        [Header("Settings")] [SerializeField] private float damage = 5f;
        [SerializeField] private float speed = 100f;

        private void Start()
        {
            Destroy(gameObject, 3f);
        }

        public void ApplyForce(Vector3 direction)
        {
            GetComponent<Rigidbody>().AddForce(direction * speed);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerController>(out var playerController))
            {
                playerController.Hit(damage);
            }

            Destroy(gameObject);
        }
    }
}