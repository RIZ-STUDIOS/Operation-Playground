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
        private float maxSpeed = 6;
        private Vector3 moveDirection;
        private Camera gameCamera;
        private void Start()
        {
            controller = GetComponent<CharacterController>();
            gameCamera = GameManager.Instance.gameCamera;
        }
        void OnMove(InputValue input)
        {
            Vector2 moveInput = input.Get<Vector2>();

            Vector3 cForward = gameCamera.transform.forward;
            Vector3 cRight = gameCamera.transform.right;
            cForward.y = 0;
            cRight.y = 0;
            cForward = cForward.normalized;
            cRight = cRight.normalized;

            Vector3 forwardRelativeDir = moveInput.y * cForward;
            Vector3 rightRelativeDir = moveInput.x * cRight;

            Vector3 relativeMoveDir = forwardRelativeDir + rightRelativeDir;
            relativeMoveDir *= maxSpeed;

            controller.SimpleMove(relativeMoveDir);
        }

        void OnLook(InputValue input)
        {

        }
    }
}
