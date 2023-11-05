using OperationPlayground.ScriptableObjects;
using OperationPlayground.Weapons;
using RicTools.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player
{
    public class PlayerShooting : GenericShooter
    {
        [SerializeField, MinValue(1)]
        private int maxWeaponSlots = 1;

        [SerializeField]
        private WeaponScriptableObject defaultWeaponScriptableObject;

        [SerializeField]
        private Transform weaponHoldTransform;

        private List<Weapon> heldWeapons = new List<Weapon>();

        private PlayerManager playerManager;

        private bool triggerDown;

        public override GameTeam Team => GameTeam.TeamA;

        private void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>();

            AddWeapon(defaultWeaponScriptableObject);

            playerManager.playerInput.Shoot.Fire.performed += OnFirePerformed;
            playerManager.playerInput.Shoot.Fire.canceled += OnFireCanceled;
        }

        public bool AddWeapon(WeaponScriptableObject weaponSo)
        {
            if (heldWeapons.Count >= maxWeaponSlots) return false;

            var weaponObject = Weapon.CreateWeapon(weaponSo, weaponHoldTransform);
            weaponObject.transform.localRotation = Quaternion.identity;
            var weapon = weaponObject.GetComponent<Weapon>();
            heldWeapons.Add(weapon);
            SwitchWeapon(weapon);

            return true;
        }

        private void OnFirePerformed(InputAction.CallbackContext value)
        {
            triggerDown = true;
        }

        private void OnFireCanceled(InputAction.CallbackContext value)
        {
            triggerDown = false;
        }

        private void Update()
        {
            if(triggerDown)
            {
                if (!currentWeapon) return;
                if (currentWeapon.Shoot())
                {
                    if (!currentWeapon.HasAmmo())
                    {
                        var newWeapon = FindWeaponWithAmmo();
                    }
                }
            }
        }

        private Weapon FindWeaponWithAmmo()
        {
            if(heldWeapons.Count == 0) return null;
            if(heldWeapons.Count == 1) return heldWeapons[0];

            for (int i = 1; i < heldWeapons.Count; i++)
            {
                if (heldWeapons[i].HasAmmo()) return heldWeapons[i];
            }

            return null;
        }
    }
}
