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

        protected Weapon _currentWeapon;
        public Weapon CurrentWeapon { get { return _currentWeapon; } }

        [SerializeField]
        private Transform weaponHoldTransform;

        public event System.Action<Weapon> onWeaponAdded;
        public event System.Action<Weapon> onWeaponSwitch;

        public void SwitchWeapon(Weapon weapon)
        {
            if (_currentWeapon)
            {
                _currentWeapon.gameObject.SetActive(false);
                _currentWeapon.SetShooter(null);
            }
            _currentWeapon = weapon;
            _currentWeapon.gameObject.SetActive(true);
            _currentWeapon.SetShooter(this);
            onWeaponSwitch?.Invoke(_currentWeapon);
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

        protected void SetWeapon(Weapon weapon)
        {
            onWeaponAdded?.Invoke(weapon);
            SwitchWeapon(weapon);
        }

        protected virtual bool CanAddWeapon(WeaponScriptableObject weaponSo)
        {
            return true;
        }

        public void HideWeapon()
        {
            if (!_currentWeapon) return;
            _currentWeapon.gameObject.SetActive(false);
        }

        public void ShowWeapon()
        {
            if (!_currentWeapon) return;
            _currentWeapon.gameObject.SetActive(true);
        }
    }
}
