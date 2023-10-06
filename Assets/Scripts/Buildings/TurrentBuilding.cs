using OperationPlayground.Projectiles;
using OperationPlayground.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Buildings
{
    public class TurrentBuilding : BuildingHealth
    {
        [SerializeField]
        private Transform fireTransform;

        private bool triggerDown;

        [SerializeField]
        private ProjectileScriptableObject projectileSo;

        [SerializeField]
        private float reloadTime = 1;

        private float reloadTimer;

        protected override void OnPlaced()
        {
        }

        public void EnableInput(GameObject playerGameObject)
        {
            var playerInputManager = playerGameObject.GetComponent<PlayerInputManager>();
            playerInputManager.playerInput.Player.Fire.performed += OnFirePerformed;
            playerInputManager.playerInput.Player.Fire.canceled += OnFireCanceled;
        }

        public void DisableInput(GameObject playerGameObject)
        {
            var playerInputManager = playerGameObject.GetComponent<PlayerInputManager>();
            playerInputManager.playerInput.Player.Fire.performed -= OnFirePerformed;
            playerInputManager.playerInput.Player.Fire.canceled -= OnFireCanceled;
            triggerDown = false;
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
                Shoot();
            }
        }

        public void Shoot()
        {
            if (reloadTimer <= 0)
            {
                reloadTimer = reloadTime;
                ShootProjectile();
            }

            reloadTimer -= Time.deltaTime;
            reloadTimer = Mathf.Max(0, reloadTimer);
        }

        private void ShootProjectile()
        {
            var gameObject = Instantiate(projectileSo.prefab);

            gameObject.transform.position = fireTransform.position;
            gameObject.transform.forward = fireTransform.forward;

            var projectile = gameObject.AddComponent<Projectile>();
            projectile.projectileSo = projectileSo;
            projectile.parentShooter = this;
        }
    }
}
