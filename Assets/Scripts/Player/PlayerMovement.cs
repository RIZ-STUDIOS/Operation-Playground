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

        private PlayerManager playerInputManager;

        private bool movementEnabled;
        private bool lookingEnabled;

        private void Awake()
        {
            playerInputManager = GetComponent<PlayerManager>();
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

        public void EnableMovement()
        {
            playerInputManager.playerInput.Player.Move.performed += OnMovePerformed;
            playerInputManager.playerInput.Player.Move.canceled += OnMoveCanceled;
            movementEnabled = true;
        }

        public void DisableMovement()
        {
            playerInputManager.playerInput.Player.Move.performed -= OnMovePerformed;
            playerInputManager.playerInput.Player.Move.canceled -= OnMoveCanceled;
            movementEnabled = false;
            moveDirection = Vector3.zero;
        }

        public void EnableLooking()
        {
            playerInputManager.playerInput.Player.Look.performed += OnLookPerformed;
        }

        public void DisableLooking()
        {
            playerInputManager.playerInput.Player.Look.performed -= OnLookPerformed;
            lookingEnabled = false;
        }

        private void EnableInput()
        {
            if (movementEnabled)
                EnableMovement();
            if (lookingEnabled)
                EnableLooking();
        }

        private void DisableInput()
        {
            DisableMovement();
            DisableLooking();
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

        public void SetPosition(Vector3 position)
        {
            if (!controller) controller = GetComponent<CharacterController>();
            controller.enabled = false;
            transform.position = position;
            controller.enabled = true;
        }
    }
}
