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
        private GameObject bulletPrefab;

        [SerializeField]
        private GameObject firingPoint;

        private OPPlayerInput playerInput;

        private float cooldownTime = 1;

        private bool triggerPressed;
        private bool canShoot = true;

        private void Awake()
        {
            playerInput = new OPPlayerInput();
            playerInput.Player.Fire.performed += OnFirePerformed;
            playerInput.Player.Fire.canceled += OnFireCanceled;
        }

        private void Update()
        {
            Debug.Log(triggerPressed);
            if (triggerPressed)
            {
                if (canShoot) ShootBullet();
            }
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
            Bullet newBullet = Instantiate(bulletPrefab).GetComponent<Bullet>();
            newBullet.gameObject.transform.position = firingPoint.transform.position;
            newBullet.gameObject.transform.rotation = firingPoint.transform.rotation;
            newBullet.Fire(firingPoint.transform.forward.normalized);
            StartCoroutine(ShootCooldown());
        }

        private IEnumerator ShootCooldown()
        {
            canShoot = false;

            float timer = 0;
            while (timer < cooldownTime)
            {
                yield return null;
            }

            canShoot = true;
        }
    }
}
