using OperationPlayground.Managers;
using OperationPlayground.Player;
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

        private void OnDrawGizmos()
        {
            if (!rotateTransform) return;
            Gizmos.DrawFrustum(rotateTransform.position, restrictedAngle, 100, 0, 1);
        }

        public void EnableRotation(PlayerManager player)
        {
            player.playerInput.Player.Look.performed += OnLookPerformed;
        }

        public void DisableRotation(PlayerManager player)
        {
            player.playerInput.Player.Look.performed -= OnLookPerformed;
            rotateTransform.localRotation = Quaternion.identity;
        }

        private void OnLookPerformed(InputAction.CallbackContext callback)
        {
            var value = callback.ReadValue<Vector2>();
            if (value.magnitude > 0.1f)
            {
                var transformRotation = transform.rotation.eulerAngles;
                if (transformRotation.y >= 180)
                    transformRotation.y -= 360;

                var cameraVector = Quaternion.Euler(0, -transformRotation.y, 0) * GameManager.Instance.gameCamera.WorldToCameraVector(value);

                var yRotation = Mathf.Atan2(cameraVector.x, cameraVector.z) * Mathf.Rad2Deg;

                var min = -restrictedAngle / 2f;
                var max = restrictedAngle / 2f;

                yRotation = Mathf.Clamp(yRotation, min, max);
                var rotation = Quaternion.Euler(0, yRotation, 0);

                rotateTransform.localRotation = rotation;
            }
        }
    }
}
