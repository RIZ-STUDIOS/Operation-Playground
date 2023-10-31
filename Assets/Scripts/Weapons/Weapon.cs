using OperationPlayground.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Weapons
{
    public class Weapon : MonoBehaviour
    {
        private WeaponScriptableObject weaponSo;

        [SerializeField]
        private int startAmmo;

        [SerializeField]
        private bool infiniteAmmo;

        private int currentAmmo;

        private float shootCooldown;

        public static GameObject CreateWeapon(WeaponScriptableObject weaponScriptableObject)
        {
            return null;
        }

        public bool Shoot()
        {
            if (currentAmmo <= 0 && !infiniteAmmo) return false;
            if(shootCooldown < weaponSo.cooldown) return false;

            return true;
        }
    }
}
