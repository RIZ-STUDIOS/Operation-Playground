using OperationPlayground.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player
{
    public class PlayerMovementTPS : MonoBehaviour
    {
        [SerializeField]
        private float maxSpeed = 6;

        [SerializeField]
        private float lookSensitivity = 12;

        private PlayerManager playerManager;

        private CharacterController controller;

        private Vector2 moveDirection;

        // Rotation inputs
        private float rotX;
        private float rotY;

        private bool isLooking;

        [SerializeField]
        private Transform cameraTarget;

        [SerializeField]
        private Transform _aimTransform;

        public Transform AimTransform { get { return _aimTransform; } }

        private void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            controller = GetComponent<CharacterController>();

            playerManager.playerInput.Movement.Move.performed += OnMovePerformed;
            playerManager.playerInput.Movement.Move.canceled += OnMoveCanceled;

            playerManager.playerInput.Movement.Look.performed += OnLookPerformed;
            playerManager.playerInput.Movement.Look.canceled += OnLookCanceled;
        }

        private void Update()
        {
            MoveCharacter();

            if (isLooking) RotateCharacter();
        }

        private void FixedUpdate()
        {
            SetAimPosition();
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
            isLooking = true;
        }

        private void OnLookCanceled(InputAction.CallbackContext value)
        {
            isLooking = false;
        }

        private void RotateCharacter()
        {
            Vector2 rotValue = playerManager.playerInput.Movement.Look.ReadValue<Vector2>();
            
            rotY += rotValue.x * Time.deltaTime * lookSensitivity;
            rotX -= rotValue.y * Time.deltaTime * (lookSensitivity / 2);

            rotX = Mathf.Clamp(rotX, -90f, 90f);

            playerManager.playerTransform.rotation = Quaternion.Euler(0, rotY, 0);
            cameraTarget.rotation = Quaternion.Euler(rotX, rotY, 0);
        }

        private void SetAimPosition()
        {
            var screenPos = new Vector3(Screen.width / 2, Screen.height / 2, 10f);
            Ray ray = playerManager.PlayerCamera.camera.ScreenPointToRay(screenPos);

            if (Physics.Raycast(ray, out RaycastHit hit, 999, 1, QueryTriggerInteraction.Ignore))
            {
                _aimTransform.position = hit.point;
            }
            else
            {
                _aimTransform.position = playerManager.PlayerCamera.camera.transform.position + playerManager.PlayerCamera.camera.transform.forward * 200f;
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
