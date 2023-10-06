using OperationPlayground.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Buildings
{
    public class TurretRotation : MonoBehaviour
    {
        [SerializeField]
        private Transform rotateTransform;

        [SerializeField]
        private float restrictedAngle;
        private Vector3 cForwardNorm;
        private Vector3 cRightNorm;

        private void OnDrawGizmos()
        {
            if (!rotateTransform) return;
            Gizmos.DrawFrustum(rotateTransform.position, restrictedAngle, 100, 0, 1);
        }

        public void EnableInput(GameObject playerGameObject)
        {
            var playerInputManager = playerGameObject.GetComponent<PlayerInputManager>();
            playerInputManager.playerInput.Player.Look.performed += OnLookPerformed;
        }

        public void DisableInput(GameObject playerGameObject)
        {
            var playerInputManager = playerGameObject.GetComponent<PlayerInputManager>();
            playerInputManager.playerInput.Player.Look.performed -= OnLookPerformed;
            rotateTransform.localRotation = Quaternion.identity;
        }

        private void OnLookPerformed(InputAction.CallbackContext callback)
        {
            var value = callback.ReadValue<Vector2>();
            if (value.magnitude > 0.1f)
            {
                var cameraVector = GameManager.Instance.gameCamera.WorldToCameraVector(value);
                var yRotation = Mathf.Atan2(cameraVector.x, cameraVector.z) * Mathf.Rad2Deg;

                var min = -restrictedAngle / 2f;
                var max = restrictedAngle / 2f;

                var transformRotation = transform.rotation.eulerAngles;
                min += transformRotation.y;
                max += transformRotation.y;

                yRotation = Mathf.Clamp(yRotation, min, max);
                var rotation = Quaternion.Euler(0, yRotation, 0);

                rotateTransform.rotation = rotation;
            }
        }

        private void UpdateNormalizedCameraVectors()
        {
            Vector3 cForward = transform.forward;
            Vector3 cRight = transform.right;
            cForward.y = 0;
            cRight.y = 0;
            cForwardNorm = cForward.normalized;
            cRightNorm = cRight.normalized;
        }

        public Vector3 WorldToCameraVector(Vector3 vector)
        {
            Vector3 forwardRelativeDir = vector.y * cForwardNorm;
            Vector3 rightRelativeDir = vector.x * cRightNorm;

            Vector3 relativeMoveDir = forwardRelativeDir + rightRelativeDir;
            return relativeMoveDir;
        }
    }
}
