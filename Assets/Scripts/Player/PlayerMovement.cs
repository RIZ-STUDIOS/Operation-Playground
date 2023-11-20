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
        }

        private void EnableInput()
        {
            playerManager.playerInput.Movement.Move.performed += OnMovePerformed;
            playerManager.playerInput.Movement.Move.canceled += OnMoveCanceled;
        }

        private void DisableInput()
        {
            playerManager.playerInput.Movement.Move.performed -= OnMovePerformed;
            playerManager.playerInput.Movement.Move.canceled -= OnMoveCanceled;
        }

        private void OnEnable()
        {
            EnableInput();
        }

        private void OnDisable()
        {
            DisableInput();
        }

        private void Update()
        {
            MoveCharacter();
        }

        private void MoveCharacter()
        {
            if (!controller.enabled) return;
            var direction = new Vector3(moveDirection.x, 0, moveDirection.y);
            if (playerManager.HasPlayerState(PlayerCapabilities.PlayerCapabilityType.TPSLook))
                direction = (playerManager.playerTransform.forward * moveDirection.y) + (playerManager.playerTransform.right * moveDirection.x);

            controller.SimpleMove(direction * maxSpeed);
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
    }
}
