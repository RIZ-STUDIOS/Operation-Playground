using OperationPlayground.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player
{
    public class PlayerMap : MonoBehaviour
    {
        private PlayerManager playerManager;

        private Vector3 cameraOffset;
        private Quaternion cameraRotation;

        private bool isZoomedOut;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();

            cameraOffset = playerManager.PlayerCamera.transform.localPosition;
            cameraRotation = playerManager.PlayerCamera.transform.rotation;

            playerManager.playerInput.Basic.ZoomMap.performed += OnZoomMapPerformed;
        }

        private void OnZoomMapPerformed(InputAction.CallbackContext context)
        {
            isZoomedOut = !isZoomedOut;

            if (isZoomedOut)
            {
                playerManager.PlayerCamera.transform.parent = null;
                playerManager.PlayerCamera.transform.position = GameManager.Instance.gameLevelData.mapCamera.position;
                playerManager.PlayerCamera.transform.rotation = GameManager.Instance.gameLevelData.mapCamera.rotation;
            }
            else
            {
                ResetZoom();
            }
        }

        private void ResetZoom()
        {
            playerManager.PlayerCamera.transform.parent = playerManager.transform;
            playerManager.PlayerCamera.transform.localPosition = cameraOffset;
            playerManager.PlayerCamera.transform.rotation = cameraRotation;
            isZoomedOut = false;
        }
    }
}
