using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        private float maxSpeed = 6;

        private PlayerManager playerManager;

        private CharacterController controller;

        private Vector2 moveDirection;

        private void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            controller = GetComponent<CharacterController>();

            playerManager.playerInput.Movement.Move.performed += OnMovePerformed;
            playerManager.playerInput.Movement.Move.canceled += OnMoveCanceled;

            playerManager.playerInput.Movement.Look.performed += OnLookPerformed;
        }

        private void Update()
        {
            MoveCharacter();
        }

        private void MoveCharacter()
        {
            if (!controller.enabled) return;
            var direction = playerManager.PlayerCamera.WorldToCameraVector(moveDirection) * maxSpeed;

            controller.SimpleMove(direction);
        }

        /// <summary>
        /// Left stick moves the player relative to the camera.
        /// </summary>
        /// <param name="value"></param>
        private void OnMovePerformed(InputAction.CallbackContext value)
        {
            moveDirection = value.ReadValue<Vector2>();
        }

        private void OnMoveCanceled(InputAction.CallbackContext value)
        {
            moveDirection = value.ReadValue<Vector2>();
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
                playerManager.playerTransform.rotation = Quaternion.LookRotation(playerManager.PlayerCamera.WorldToCameraVector(vector), Vector3.up);
            }
        }

        public void SetPosition(Vector3 position)
        {
            if (!controller) controller = GetComponentInParent<CharacterController>();
            var enabled = controller.enabled;
            controller.enabled = false;
            transform.position = position;
            controller.enabled = enabled;
        }
    }
}
