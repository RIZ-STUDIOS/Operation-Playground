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
        private BuildingData buildingData;

        [SerializeField]
        private Transform rotateTransform;

        [SerializeField]
        private float restrictedAngle;

        private PlayerManager currentPlayer;

        private void Awake()
        {
            buildingData = GetComponent<BuildingData>();

            buildingData.onPlayerEnterBuilding += (playerManager) =>
            {
                currentPlayer = playerManager;
            };

            buildingData.onPlayerLeaveBuilding += (_) =>
            {
                currentPlayer = null;
            };

            buildingData.onLookPerformed += OnLookPerformed;
        }

        private void OnDrawGizmos()
        {
            if (!rotateTransform) return;
            Gizmos.DrawFrustum(rotateTransform.position, restrictedAngle, 100, 0, 1);
        }

        private void OnLookPerformed(InputAction.CallbackContext callback)
        {
            var value = callback.ReadValue<Vector2>();
            if (value.magnitude > 0.1f)
            {
                var transformRotation = transform.rotation.eulerAngles;
                if (transformRotation.y >= 180)
                    transformRotation.y -= 360;

                var cameraVector = Quaternion.Euler(0, -transformRotation.y, 0) * currentPlayer.PlayerCamera.WorldToCameraVector(value);

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
