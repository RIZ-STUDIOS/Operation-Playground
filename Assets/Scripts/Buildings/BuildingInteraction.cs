using OperationPlayground.Interactables;
using OperationPlayground.Player;
using OperationPlayground.Player.PlayerCapabilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Buildings
{
    public class BuildingInteraction : MonoBehaviour
    {
        private Interactable interactable;

        public System.Action<PlayerManager> onEnterBuilding;
        public System.Action<PlayerManager> onExitBuilding;

        private PlayerManager playerManager;

        private void Awake()
        {
            interactable = GetComponent<Interactable>();
            interactable.onInteract += OnEnterBuilding;
        }

        private void OnEnterBuilding(PlayerManager player)
        {
            if (playerManager) return;
            playerManager = player;
            player.playerInput.Player.Interact.performed += OnInteractPerformed;
            player.RemovePlayerState(PlayerCapabilityType.Building);
            player.RemovePlayerState(PlayerCapabilityType.Looking);
            player.RemovePlayerState(PlayerCapabilityType.Movement);
            player.RemovePlayerState(PlayerCapabilityType.Shooting);
            player.RemovePlayerState(PlayerCapabilityType.HealthBar);
            player.RemovePlayerState(PlayerCapabilityType.InvalidPlacement);
            player.RemovePlayerState(PlayerCapabilityType.EnemyTarget);
            player.RemovePlayerState(PlayerCapabilityType.Interaction);
            player.RemovePlayerState(PlayerCapabilityType.Graphics);
            player.RemovePlayerState(PlayerCapabilityType.Collision);
            onEnterBuilding?.Invoke(player);
        }

        private void OnInteractPerformed(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();

            var button = PlayerInteraction.GetInteractionButton(value);

            if (button == InteractionButton.Right)
            {
                OnExitBuilding();
            }
        }

        public void OnExitBuilding()
        {
            if (!playerManager) return;
            playerManager.playerInput.Player.Interact.performed -= OnInteractPerformed;
            playerManager.AddPlayerState(PlayerCapabilityType.Building);
            playerManager.AddPlayerState(PlayerCapabilityType.Looking);
            playerManager.AddPlayerState(PlayerCapabilityType.Movement);
            playerManager.AddPlayerState(PlayerCapabilityType.Shooting);
            playerManager.AddPlayerState(PlayerCapabilityType.HealthBar);
            playerManager.AddPlayerState(PlayerCapabilityType.InvalidPlacement);
            playerManager.AddPlayerState(PlayerCapabilityType.EnemyTarget);
            playerManager.AddPlayerState(PlayerCapabilityType.Interaction);
            playerManager.AddPlayerState(PlayerCapabilityType.Graphics);
            playerManager.AddPlayerState(PlayerCapabilityType.Collision);
            onExitBuilding?.Invoke(playerManager);
            playerManager = null;
        }
    }
}
