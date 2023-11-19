using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player
{
    public class PlayerLookMap : MonoBehaviour
    {
        private PlayerManager playerManager;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
        }

        private void EnableInput()
        {
            playerManager.playerInput.Movement.Look.performed += OnLookPerformed;
        }

        private void DisableInput()
        {
            playerManager.playerInput.Movement.Look.performed -= OnLookPerformed;
        }

        private void OnEnable()
        {
            EnableInput();
        }

        private void OnDisable()
        {
            DisableInput();
        }

        /// <summary>
        /// Right stick rotates the player relative to the camera.
        /// </summary>
        /// <param name="value"></param>
        private void OnLookPerformed(InputAction.CallbackContext value)
        {
            var vector = value.ReadValue<Vector2>();
            if (vector.magnitude > 0.1f)
            {
                playerManager.playerTransform.rotation = Quaternion.LookRotation(new Vector3(vector.x, 0, vector.y), Vector3.up);
            }
        }
    }
}
