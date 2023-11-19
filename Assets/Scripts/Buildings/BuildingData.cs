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

        public event System.Action<PlayerManager> onPlayerEnterBuilding;
        public event System.Action<PlayerManager> onPlayerLeaveBuilding;

        public event System.Action<InputAction.CallbackContext> onFirePerformed;
        public event System.Action<InputAction.CallbackContext> onFireCanceled;

        public event System.Action<InputAction.CallbackContext> onLookPerformed;
        public event System.Action<InputAction.CallbackContext> onLookCanceled;

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

            currentPlayer.playerInput.InBuild.Fire.performed += OnFirePerformed;
            currentPlayer.playerInput.InBuild.Fire.canceled += OnFireCanceled;

            currentPlayer.playerInput.InBuild.Look.performed += OnLookPerformed;
            currentPlayer.playerInput.InBuild.Look.canceled += OnLookCanceled;

            interactable.CanInteractWith = false;

            playerPositionInvalidPlacement = new GameObject();
            playerPositionInvalidPlacement.layer = 8;
            playerPositionInvalidPlacement.transform.position = playerPosition;
            playerPositionInvalidPlacement.AddComponent<BoxCollider>();
            playerPositionInvalidPlacement.AddComponent<InvalidPlacement>();

            onPlayerEnterBuilding?.Invoke(playerManager);
        }

        private void OnLeavePerformed(InputAction.CallbackContext context)
        {
            currentPlayer.playerInput.InBuild.Leave.performed -= OnLeavePerformed;

            currentPlayer.playerInput.InBuild.Fire.performed -= OnFirePerformed;
            currentPlayer.playerInput.InBuild.Fire.canceled -= OnFireCanceled;

            currentPlayer.playerInput.InBuild.Look.performed -= OnLookPerformed;
            currentPlayer.playerInput.InBuild.Look.canceled -= OnLookCanceled;

            currentPlayer.AddPlayerState(PlayerCapabilityType.Movement);
            currentPlayer.AddPlayerState(PlayerCapabilityType.Interaction);
            currentPlayer.AddPlayerState(PlayerCapabilityType.Shooter);
            currentPlayer.AddPlayerState(PlayerCapabilityType.Collision);
            currentPlayer.AddPlayerState(PlayerCapabilityType.Graphics);
            currentPlayer.AddPlayerState(PlayerCapabilityType.Health);
            currentPlayer.AddPlayerState(PlayerCapabilityType.InvalidPlacement);
            currentPlayer.AddPlayerState(PlayerCapabilityType.ToggleBuilding);

            currentPlayer.SetPosition(playerPosition);

            onPlayerLeaveBuilding?.Invoke(currentPlayer);

            currentPlayer = null;
            interactable.CanInteractWith = true;

            Destroy(playerPositionInvalidPlacement);
        }

        private void OnFirePerformed(InputAction.CallbackContext callback)
        {
            onFirePerformed?.Invoke(callback);
        }

        private void OnFireCanceled(InputAction.CallbackContext callback)
        {
            onFireCanceled?.Invoke(callback);
        }

        private void OnLookPerformed(InputAction.CallbackContext callback)
        {
            onLookPerformed?.Invoke(callback);
        }

        private void OnLookCanceled(InputAction.CallbackContext callback)
        {
            onLookCanceled?.Invoke(callback);
        }
    }
}
