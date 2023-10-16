using OperationPlayground.Player;
using OperationPlayground.Projectiles;
using OperationPlayground.ScriptableObjects;
using OperationPlayground.Weapons;
using RicTools.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Buildings
{
    public class TurrentBuilding : BuildingHealth
    {
        private bool triggerDown;

        private TurretRotation turretRotation;

        private Weapon turretWeapon;

        private PlayerManager playerManager;

        protected override void Awake()
        {
            base.Awake();
            turretWeapon = GetComponent<Weapon>();
            turretRotation = GetComponent<TurretRotation>();
            buildingInteraction.onEnterBuilding += EnableFiring;
            buildingInteraction.onExitBuilding += DisableFiring;

            turretWeapon.parentShooter = this;
        }

        protected override void OnPlaced()
        {
        }

        public void EnableFiring(PlayerManager player)
        {
            player.playerInput.Player.Fire.performed += OnFirePerformed;
            player.playerInput.Player.Fire.canceled += OnFireCanceled;
            turretRotation.EnableRotation(player);
            playerManager = player;
        }

        public void DisableFiring(PlayerManager player)
        {
            player.playerInput.Player.Fire.performed -= OnFirePerformed;
            player.playerInput.Player.Fire.canceled -= OnFireCanceled;
            turretRotation.DisableRotation(player);
            triggerDown = false;
            playerManager = null;
        }

        private void OnFirePerformed(InputAction.CallbackContext context)
        {
            triggerDown = true;
        }

        private void OnFireCanceled(InputAction.CallbackContext context)
        {
            triggerDown = false;
        }

        private void Update()
        {
            if (triggerDown)
            {
                if (turretWeapon.Shoot())
                {
                    playerManager.rumbleController.DoRumble(0.1f, 0.4f, turretWeapon.reloadTime / 2f);
                }
            }
        }
    }
}
