using OperationPlayground.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player
{
    //[RequireComponent(typeof(PlayerInput))]
    public class PlayerMovement : MonoBehaviour
    {

        [SerializeField]
        private float maxSpeed = 6;

        private CharacterController controller;

        private Vector3 moveDirection;

        private GameCamera gameCamera;

        private PlayerInputManager playerInputManager;

        private void Awake()
        {
            playerInputManager = GetComponent<PlayerInputManager>();
            controller = GetComponent<CharacterController>();
        }

        private void Start()
        {
            gameCamera = GameManager.Instance.gameCamera;
        }

        private void Update()
        {
            MoveCharacter();
        }

        private void MoveCharacter()
        {
            var direction = gameCamera.WorldToCameraVector(moveDirection) * maxSpeed;

            controller.SimpleMove(direction);
        }

        private void OnEnable()
        {
            EnableInput();
        }

        private void OnDisable()
        {
            DisableInput();
        }

        private void EnableInput()
        {
            playerInputManager.playerInput.Player.Move.performed += OnMovePerformed;
            playerInputManager.playerInput.Player.Move.canceled += OnMoveCanceled;

            playerInputManager.playerInput.Player.Look.performed += OnLookPerformed;
        }

        private void DisableInput()
        {
            playerInputManager.playerInput.Player.Move.performed -= OnMovePerformed;
            playerInputManager.playerInput.Player.Move.canceled -= OnMoveCanceled;

            playerInputManager.playerInput.Player.Look.performed -= OnLookPerformed;
            moveDirection = Vector3.zero;
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
            moveDirection = Vector3.zero;
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
                transform.rotation = Quaternion.LookRotation(gameCamera.WorldToCameraVector(vector), Vector3.up);
            }
        }
    }
}
