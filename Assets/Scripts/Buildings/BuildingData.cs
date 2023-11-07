using OperationPlayground.EntityData;
using OperationPlayground.Interactables;
using OperationPlayground.Player;
using OperationPlayground.Player.PlayerCapabilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Buildings
{
    [RequireComponent(typeof(Interactable))]
    public class BuildingData : GenericEntity
    {
        public override GenericHealth Health => null;

        public override GenericShooter Shooter => null;

        private Interactable interactable;

        private PlayerManager currentPlayer;

        private Vector3 playerPosition;

        public override GameTeam Team => GameTeam.TeamA;

        private GameObject playerPositionInvalidPlacement;

        protected override void Awake()
        {
            base.Awake();

            interactable = GetComponent<Interactable>();

            interactable.onInteract += OnInteract;
        }

        private void OnInteract(PlayerManager playerManager)
        {
            if (currentPlayer) return;
            currentPlayer = playerManager;

            currentPlayer.RemovePlayerState(PlayerCapabilityType.Movement);
            currentPlayer.RemovePlayerState(PlayerCapabilityType.Interaction);
            currentPlayer.RemovePlayerState(PlayerCapabilityType.Building);
            currentPlayer.RemovePlayerState(PlayerCapabilityType.Shooter);
            currentPlayer.RemovePlayerState(PlayerCapabilityType.Collision);
            currentPlayer.RemovePlayerState(PlayerCapabilityType.Graphics);
            currentPlayer.RemovePlayerState(PlayerCapabilityType.Health);
            currentPlayer.RemovePlayerState(PlayerCapabilityType.InvalidPlacement);
            currentPlayer.RemovePlayerState(PlayerCapabilityType.ToggleBuilding);

            playerPosition = playerManager.transform.position;
            currentPlayer.SetPosition(transform.position);

            currentPlayer.playerInput.InBuild.Leave.performed += OnLeavePerformed;

            interactable.CanInteractWith = false;

            playerPositionInvalidPlacement = new GameObject();
            playerPositionInvalidPlacement.layer = 8;
            playerPositionInvalidPlacement.transform.position = playerPosition;
            playerPositionInvalidPlacement.AddComponent<BoxCollider>();
            playerPositionInvalidPlacement.AddComponent<InvalidPlacement>();
        }

        private void OnLeavePerformed(InputAction.CallbackContext context)
        {
            currentPlayer.playerInput.InBuild.Leave.performed -= OnLeavePerformed;

            currentPlayer.AddPlayerState(PlayerCapabilityType.Movement);
            currentPlayer.AddPlayerState(PlayerCapabilityType.Interaction);
            currentPlayer.AddPlayerState(PlayerCapabilityType.Shooter);
            currentPlayer.AddPlayerState(PlayerCapabilityType.Collision);
            currentPlayer.AddPlayerState(PlayerCapabilityType.Graphics);
            currentPlayer.AddPlayerState(PlayerCapabilityType.Health);
            currentPlayer.AddPlayerState(PlayerCapabilityType.InvalidPlacement);
            currentPlayer.AddPlayerState(PlayerCapabilityType.ToggleBuilding);

            currentPlayer.SetPosition(playerPosition);
            currentPlayer = null;
            interactable.CanInteractWith = true;

            Destroy(playerPositionInvalidPlacement);
        }
    }
}
