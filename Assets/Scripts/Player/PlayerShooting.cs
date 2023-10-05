using OperationPlayground.ScriptableObjects;
using OperationPlayground.Weapons.Projectiles;
using RicTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerShooting : MonoBehaviour
    {
        [SerializeField]
        private ProjectileScriptableObject projectileSo;

        [SerializeField]
        private Transform firingPoint;

        private OPPlayerInput playerInput;

        private float cooldownTime = 1;
        private float cooldownTimer;

        private bool triggerPressed;
        private bool canShoot = true;

        private void Awake()
        {
            playerInput = new OPPlayerInput();
            playerInput.Enable();
            playerInput.Player.Fire.performed += OnFirePerformed;
            playerInput.Player.Fire.canceled += OnFireCanceled;
        }

        private void Update()
        {
            if(triggerPressed && cooldownTimer <= 0)
            {
                cooldownTimer = cooldownTime;
                ShootBullet();
            }

            cooldownTimer -= Time.deltaTime;
            cooldownTimer = Mathf.Max(0, cooldownTimer);
        }

        private void OnFirePerformed(InputAction.CallbackContext value)
        {
            triggerPressed = true;
        }

        private void OnFireCanceled(InputAction.CallbackContext value)
        {
            triggerPressed = false;
        }

        private void ShootBullet()
        {
            var gameObject = Instantiate(projectileSo.prefab);

            gameObject.transform.position = firingPoint.position;
            gameObject.transform.forward = firingPoint.forward;

            var projectile = gameObject.AddComponent<Projectile>();
            projectile.projectileSo = projectileSo;
        }
    }
}
