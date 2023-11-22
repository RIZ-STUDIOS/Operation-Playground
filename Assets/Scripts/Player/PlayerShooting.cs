using OperationPlayground.ScriptableObjects;
using OperationPlayground.Weapons;
using RicTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace OperationPlayground.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerShooting : MonoBehaviour
    {
        [SerializeField]
        private Transform weaponSlotTransform;

        private bool triggerPressed;

        [SerializeField]
        private Weapon defaultWeapon;

        private Weapon equippedWeapon;

        private PlayerManager playerInputManager;

        private bool shootingEnabled;

        public Weapon EquippedWeapon
        {
            get
            {
                if (equippedWeapon) return equippedWeapon;
                return defaultWeapon;
            }
        }

        private void Awake()
        {
            playerInputManager = GetComponent<PlayerManager>();

            EquipWeapon(defaultWeapon);
        }

        private void Update()
        {
            if (triggerPressed)
            {
                EquippedWeapon.Shoot();
            }
        }

        private void OnEnable()
        {
            EnableInput();
        }

        private void OnDisable()
        {
            DisableInput();
        }

        public void EnableShooting()
        {
            playerInputManager.playerInput.Player.Fire.performed += OnFirePerformed;
            playerInputManager.playerInput.Player.Fire.canceled += OnFireCanceled;
            weaponSlotTransform.gameObject.SetActive(true);
            shootingEnabled = true;
        }

        public void DisableShooting()
        {
            playerInputManager.playerInput.Player.Fire.performed -= OnFirePerformed;
            playerInputManager.playerInput.Player.Fire.canceled -= OnFireCanceled;
            weaponSlotTransform.gameObject.SetActive(false);
            triggerPressed = false;
            shootingEnabled = false;
        }

        private void EnableInput()
        {
            if (shootingEnabled)
                EnableShooting();
        }

        private void DisableInput()
        {
            DisableShooting();
        }

        private void OnFirePerformed(InputAction.CallbackContext value)
        {
            triggerPressed = true;
        }

        private void OnFireCanceled(InputAction.CallbackContext value)
        {
            triggerPressed = false;
        }

        public bool EquipWeapon(Weapon weapon)
        {
            if (weapon == null)
            {
                if (equippedWeapon == null || equippedWeapon == defaultWeapon) return false;

                Destroy(equippedWeapon.gameObject);

                equippedWeapon = null;

                return EquipWeapon(defaultWeapon);
            }

            if (equippedWeapon != null && equippedWeapon != defaultWeapon) return false;

            if (weapon != defaultWeapon)
                defaultWeapon.gameObject.SetActive(false);
            equippedWeapon = weapon;
            weapon.transform.SetParent(weaponSlotTransform, false);
            weapon.transform.localPosition = weapon.offset;
            weapon.onAmmoEnd += UnequipWeapon;
            weapon.parentShooter = GetComponent<ObjectHealth>();
            equippedWeapon.gameObject.SetActive(true);
            return true;
        }

        private void UnequipWeapon()
        {
            EquipWeapon(null);
        }
    }
}