using OperationPlayground.Managers;
using OperationPlayground.Player.PlayerCapabilities;
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

        [SerializeField]
        private GameObject mapCamera;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();

            cameraOffset = playerManager.PlayerCamera.transform.localPosition;
            cameraRotation = playerManager.PlayerCamera.transform.rotation;

            playerManager.playerInput.Basic.ZoomMap.performed += OnZoomMapPerformed;
        }

        private void Start()
        {
            mapCamera = GameManager.Instance.gameLevelData.mapCamera.gameObject;
        }

        private void OnZoomMapPerformed(InputAction.CallbackContext context)
        {
            isZoomedOut = !isZoomedOut;

            if (isZoomedOut)
            {
                playerManager.PlayerCamera.cameraBrain.enabled = false;

                playerManager.PlayerCamera.transform.parent = null;
                playerManager.PlayerCamera.transform.position = GameManager.Instance.gameLevelData.mapCamera.position;
                playerManager.PlayerCamera.transform.rotation = GameManager.Instance.gameLevelData.mapCamera.rotation;

                playerManager.RemovePlayerState(PlayerCapabilityType.TPSMovement);
                playerManager.AddPlayerState(PlayerCapabilityType.MapMovement);
            }
            else
            {
                ResetZoom();
            }
        }

        private void ResetZoom()
        {
            playerManager.RemovePlayerState(PlayerCapabilityType.MapMovement);
            playerManager.AddPlayerState(PlayerCapabilityType.TPSMovement);

            playerManager.PlayerCamera.cameraBrain.enabled = true;

            playerManager.PlayerCamera.transform.parent = playerManager.transform;
            playerManager.PlayerCamera.transform.localPosition = cameraOffset;
            playerManager.PlayerCamera.transform.rotation = cameraRotation;
            isZoomedOut = false;
        }
    }
}
