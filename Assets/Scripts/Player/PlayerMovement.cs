using OperationPlayground.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player
{
    [RequireComponent(typeof(PlayerInput)) ]
    public class PlayerMovement : MonoBehaviour
    {
        private CharacterController controller;
        [SerializeField]
        private float maxSpeed = 6;
        private Vector3 moveDirection;
        private Camera gameCamera;
        private Vector3 cForwardNorm;
        private Vector3 cRightNorm;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            gameCamera = GameManager.Instance.gameCamera;
            UpdateNormalizedCameraVectors();
        }

        private void Update()
        {
            controller.SimpleMove(moveDirection);
        }

        /// <summary>
        /// Left stick moves the player relative to the camera.
        /// </summary>
        /// <param name="input"></param>
        void OnMove(InputValue input)
        {
            Vector2 moveInput = input.Get<Vector2>();

            Vector3 forwardRelativeDir = moveInput.y * cForwardNorm;
            Vector3 rightRelativeDir = moveInput.x * cRightNorm;

            Vector3 relativeMoveDir = forwardRelativeDir + rightRelativeDir;
            moveDirection = relativeMoveDir *= maxSpeed;
        }

        /// <summary>
        /// Right stick rotates the player relative to the camera.
        /// </summary>
        /// <param name="input"></param>
        void OnLook(InputValue input)
        {
            Vector2 moveInput = input.Get<Vector2>();
            if (moveInput.magnitude > 0.1f)
            {
                Vector3 forwardRelativeDir = moveInput.y * cForwardNorm;
                Vector3 rightRelativeDir = moveInput.x * cRightNorm;

                Vector3 relativeRotVector = forwardRelativeDir + rightRelativeDir;

                transform.rotation = Quaternion.LookRotation(relativeRotVector, Vector3.up);
            }
        }

        /// <summary>
        /// Should the camera move, continue to update the normalized vectors until stationary.
        /// </summary>
        private void UpdateNormalizedCameraVectors()
        {
            Vector3 cForward = gameCamera.transform.forward;
            Vector3 cRight = gameCamera.transform.right;
            cForward.y = 0;
            cRight.y = 0;
            cForwardNorm = cForward.normalized;
            cRightNorm = cRight.normalized;
        }
    }
}
