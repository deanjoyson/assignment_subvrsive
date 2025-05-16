using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleArena
{
    [CreateAssetMenu(menuName = "Weapon/New Weapon", fileName = "New Weapon")]
    public class Weapon : ScriptableObject
    {
        public float AttackSpeed;
        public float Range;
        public GameObject BulletPrefab;
    }
}