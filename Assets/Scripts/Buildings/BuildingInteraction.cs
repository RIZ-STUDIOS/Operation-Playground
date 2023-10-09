using OperationPlayground.Interactables;
using OperationPlayground.Player;
using OperationPlayground.Player.PlayerStates;
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
            player.RemovePlayerState(PlayerStateType.Building);
            player.RemovePlayerState(PlayerStateType.Looking);
            player.RemovePlayerState(PlayerStateType.Movement);
            player.RemovePlayerState(PlayerStateType.Shooting);
            player.RemovePlayerState(PlayerStateType.HealthBar);
            player.RemovePlayerState(PlayerStateType.InvalidPlacement);
            player.RemovePlayerState(PlayerStateType.EnemyTarget);
            player.RemovePlayerState(PlayerStateType.Interaction);
            player.RemovePlayerState(PlayerStateType.Graphics);
            player.RemovePlayerState(PlayerStateType.Collision);
            onEnterBuilding?.Invoke(player);
        }

        private void OnInteractPerformed(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();

            var button = PlayerInteraction.GetInteractionButton(value);

            if (button == InteractionButton.Right)
            {
                OnExitBuilding(playerManager);
            }
        }

        private void OnExitBuilding(PlayerManager player)
        {
            player.playerInput.Player.Interact.performed -= OnInteractPerformed;
            player.AddPlayerState(PlayerStateType.Building);
            player.AddPlayerState(PlayerStateType.Looking);
            player.AddPlayerState(PlayerStateType.Movement);
            player.AddPlayerState(PlayerStateType.Shooting);
            player.AddPlayerState(PlayerStateType.HealthBar);
            player.AddPlayerState(PlayerStateType.InvalidPlacement);
            player.AddPlayerState(PlayerStateType.EnemyTarget);
            player.AddPlayerState(PlayerStateType.Interaction);
            player.AddPlayerState(PlayerStateType.Graphics);
            player.AddPlayerState(PlayerStateType.Collision);
            onExitBuilding?.Invoke(player);
            playerManager = null;
        }
    }
}
