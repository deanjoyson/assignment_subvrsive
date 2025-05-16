using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleArena
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Gun currentWeapon;

        public Gun CurrentWeapon => currentWeapon;

        public void ShootAtTarget(PlayerController target)
        {
            currentWeapon.ShootBullet(target.transform.position);
        }

        public void SetWeapon(Weapon weapon)
        {
            currentWeapon.SetWeaponDetails(weapon);
        }
    }
}