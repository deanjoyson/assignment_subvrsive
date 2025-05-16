using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleArena
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Weapon weaponDetails;
        [SerializeField] private Transform spawnPoint;

        public Weapon WeaponDetails => weaponDetails;

        public void SetWeaponDetails(Weapon weapon)
        {
            weaponDetails = weapon;
        }

        public void ShootBullet(Vector3 targetPos)
        {
            var dir = targetPos - spawnPoint.position;
            var bulletObject = Instantiate(weaponDetails.BulletPrefab, spawnPoint.position, Quaternion.identity);
            if (bulletObject.TryGetComponent<Bullet>(out var bullet))
            {
                bullet.ApplyForce(dir);
            }
        }
    }
}