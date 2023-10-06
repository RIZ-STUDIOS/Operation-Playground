using OperationPlayground.Buildings;
using OperationPlayground.Interactables;
using OperationPlayground.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Buildings
{
    public class TurretBuildingInteraction : BuildingInteraction
    {
        private TurretRotation turretRotation;
        private TurrentBuilding turrentBuilding;

        private PlayerInputManager currentPlayer;

        protected override void Awake()
        {
            base.Awake();
            turretRotation = GetComponent<TurretRotation>();
            turrentBuilding = GetComponent<TurrentBuilding>();

            onEnterBuilding += turretRotation.EnableInput;
            onExitBuilding += turretRotation.DisableInput;

            onEnterBuilding += turrentBuilding.EnableInput;
            onExitBuilding += turrentBuilding.DisableInput;

            onEnterBuilding += OnEnterBuilding;
            onExitBuilding += OnExitBuilding;
        }

        private void OnEnterBuilding(GameObject playerGameObject)
        {
            currentPlayer = playerGameObject.GetComponent<PlayerInputManager>();
            currentPlayer.playerInput.Player.Interact.performed += ExitBuilding;
        }

        private void OnExitBuilding(GameObject playerGameObject)
        {
            currentPlayer.playerInput.Player.Interact.performed -= ExitBuilding;

            currentPlayer = null;
        }

        private void ExitBuilding(InputAction.CallbackContext callbackContext)
        {
            var value = callbackContext.ReadValue<Vector2>();

            var button = GetInteractButton(value);

            if (button == InteractButton.Right)
            {
                ExitBuilding(currentPlayer.gameObject);
            }
        }

        private InteractButton GetInteractButton(Vector2 data)
        {
            if (data.x == 1)
            {
                return InteractButton.Right;
            }
            else if (data.x == -1)
            {
                return InteractButton.Left;
            }

            if (data.y == 1)
            {
                return InteractButton.Top;
            }
            else if (data.y == -1)
            {
                return InteractButton.Bottom;
            }

            return InteractButton.None;
        }
    }
}
