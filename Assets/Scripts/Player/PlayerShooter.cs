using OperationPlayground.EntityData;
using OperationPlayground.ScriptableObjects;
using OperationPlayground.Weapons;
using RicTools.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player
{
    public class PlayerShooter : GenericShooter
    {
        [SerializeField, MinValue(1)]
        private int maxWeaponSlots = 1;

        [SerializeField]
        private WeaponScriptableObject defaultWeaponScriptableObject;

        private List<Weapon> heldWeapons = new List<Weapon>();

        private int heldWeaponIndex;

        private PlayerManager playerManager;

        private bool triggerDown;

        private void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>();

            playerManager.playerInput.Shoot.Fire.performed += OnFirePerformed;
            playerManager.playerInput.Shoot.Fire.canceled += OnFireCanceled;

            playerManager.playerInput.Shoot.Cycle.performed += OnCyclePerformed;

            onWeaponAdded += OnWeaponAdded;
            onWeaponSwitch += OnWeaponSwitch;

            AddWeapon(defaultWeaponScriptableObject);
        }

        protected override bool CanAddWeapon(WeaponScriptableObject weaponSo)
        {
            if (heldWeapons.Count >= maxWeaponSlots) return false;
            return heldWeapons.Find(weapon => weapon.CompareScriptableObject(weaponSo)) == null;
        }

        private void OnWeaponAdded(Weapon weapon)
        {
            heldWeapons.Add(weapon);
        }

        private void OnWeaponSwitch(Weapon weapon)
        {
            heldWeaponIndex = heldWeapons.IndexOf(weapon);
        }

        public bool SwitchWeapon(int index, WeaponScriptableObject newWeapon)
        {
            if (index >= heldWeapons.Count) return false;
            return SwitchWeapon(heldWeapons[index], newWeapon);
        }

        public bool SwitchWeapon(Weapon previousWeapon, WeaponScriptableObject newWeapon)
        {
            if (!RemoveWeapon(previousWeapon)) return false;
            return AddWeapon(newWeapon);
        }

        private bool RemoveWeapon(Weapon weapon)
        {
            if (weapon == heldWeapons[0]) return false;
            if (currentWeapon == weapon)
            {
                SwitchWeapon(heldWeapons[0]);
            }
            Destroy(weapon.gameObject);
            heldWeapons.Remove(weapon);
            return true;
        }

        private void OnFirePerformed(InputAction.CallbackContext context)
        {
            triggerDown = true;
        }

        private void OnFireCanceled(InputAction.CallbackContext context)
        {
            triggerDown = false;
        }

        private void OnCyclePerformed(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<float>();
            if (value == 0) return;

            var index = heldWeaponIndex;

            index += value > 0 ? 1 : -1;

            if (index < 0)
                index = heldWeapons.Count - 1;
            else if (index >= heldWeapons.Count)
                index = 0;

            SwitchWeapon(heldWeapons[index]);
        }

        /*private void Update()
        {
            if (triggerDown)
            {
                if (!currentWeapon) return;
                if (currentWeapon.Shoot())
                {
                    if (!currentWeapon.HasAmmo())
                    {
                        //var newWeapon = FindWeaponWithAmmo();
                    }
                }
            }
        }*/

        private void LateUpdate()
        {
            if (triggerDown)
            {
                if (!currentWeapon) return;
                if (currentWeapon.Shoot())
                {
                    if (!currentWeapon.HasAmmo())
                    {
                        //var newWeapon = FindWeaponWithAmmo();
                    }
                }
            }
        }

        private Weapon FindWeaponWithAmmo()
        {
            if (heldWeapons.Count == 0) return null;
            if (heldWeapons.Count == 1) return heldWeapons[0];

            for (int i = 1; i < heldWeapons.Count; i++)
            {
                if (heldWeapons[i].HasAmmo()) return heldWeapons[i];
            }

            return null;
        }
    }
}
