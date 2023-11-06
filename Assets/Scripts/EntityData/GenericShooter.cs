using OperationPlayground.ScriptableObjects;
using OperationPlayground.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.EntityData
{

    public abstract class GenericShooter : MonoBehaviour
    {
        [System.NonSerialized]
        public GenericEntity parentEntity;

        protected Weapon currentWeapon;

        [SerializeField]
        private Transform weaponHoldTransform;

        public event System.Action<Weapon> onWeaponAdded;
        public event System.Action<Weapon> onWeaponSwitch;

        public void SwitchWeapon(Weapon weapon)
        {
            if (weapon.interactable)
                Destroy(weapon.interactable);
            if (currentWeapon)
            {
                currentWeapon.gameObject.SetActive(false);
                currentWeapon.SetShooter(null);
            }
            currentWeapon = weapon;
            currentWeapon.gameObject.SetActive(true);
            currentWeapon.SetShooter(this);
            onWeaponSwitch?.Invoke(currentWeapon);
        }

        public bool AddWeapon(WeaponScriptableObject weaponSo)
        {
            if (!CanAddWeapon(weaponSo)) return false;

            var weaponObject = Weapon.CreateWeapon(weaponSo, weaponHoldTransform);
            var weapon = weaponObject.GetComponent<Weapon>();
            AddWeapon(weapon);

            return true;
        }

        public bool AddWeapon(Weapon weapon)
        {
            if (!CanAddWeapon(weapon.weaponSo)) return false;

            var weaponObject = weapon.gameObject;
            weaponObject.transform.localRotation = Quaternion.identity;
            weapon.transform.SetParent(weaponHoldTransform, false);
            weapon.ApplyOffset();
            onWeaponAdded?.Invoke(weapon);
            SwitchWeapon(weapon);

            return true;
        }

        protected virtual bool CanAddWeapon(WeaponScriptableObject weaponSo)
        {
            return true;
        }
    }
}
