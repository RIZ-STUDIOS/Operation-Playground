using OperationPlayground.EntityData;
using OperationPlayground.Interactables;
using OperationPlayground.Managers;
using OperationPlayground.Player;
using OperationPlayground.Player.PlayerCapabilities;
using OperationPlayground.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Buildings
{
    [RequireComponent(typeof(Interactable))]
    [RequireComponent(typeof(BuildingHealth))]
    public class BuildingData : GenericEntity
    {
        public BuildingScriptableObject buildingScriptableObject;

        public override GenericHealth Health => BuildingHealth;

        public override GenericShooter Shooter => BuildingShooter;

        public BuildingHealth BuildingHealth => this.GetIfNull(ref _buildingHealth);

        public BuildingShooter BuildingShooter => this.GetIfNull(ref _buildingShooter);

        private BuildingHealth _buildingHealth;

        private BuildingShooter _buildingShooter;

        private Interactable interactable;

        private PlayerManager currentPlayer;

        private Vector3 playerPosition;
        private Quaternion playerRotation;

        public override GameTeam Team => GameTeam.TeamA;

        private GameObject playerPositionInvalidPlacement;

        public event System.Action<PlayerManager> onPlayerEnterBuilding;
        public event System.Action<PlayerManager> onPlayerLeaveBuilding;

        public event System.Action<InputAction.CallbackContext> onFirePerformed;
        public event System.Action<InputAction.CallbackContext> onFireCanceled;

        public event System.Action<InputAction.CallbackContext> onLookPerformed;
        public event System.Action<InputAction.CallbackContext> onLookCanceled;

        [SerializeField]
        private Transform cameraTargetTransform;

        protected override void Awake()
        {
            base.Awake();

            interactable = GetComponent<Interactable>();

            interactable.onInteract += OnInteract;

            Health.OnDeath += () =>
            {
                if (!currentPlayer) return;

                if (currentPlayer.PlayerInteraction.CurrentInteractable == interactable)
                    currentPlayer.PlayerInteraction.SetInteractable(null);
            };


            Health.OnDeath += RemovePlayer;

            GameStateManager.Instance.OnGameOver += RemovePlayer;
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
            currentPlayer.RemovePlayerState(PlayerCapabilityType.GunVisibility);
            currentPlayer.RemovePlayerState(PlayerCapabilityType.Target);

            playerPosition = playerManager.transform.position;
            playerRotation = playerManager.playerTransform.rotation;
            currentPlayer.SetPosition(transform.position);

            playerManager.PlayerCamera.CameraTarget = cameraTargetTransform ?? transform;

            playerManager.PlayerCamera.CameraCollider.enabled = false;

            playerManager.playerTransform.rotation = transform.rotation;


            currentPlayer.playerInput.InBuild.Leave.performed += OnLeavePerformed;

            currentPlayer.playerInput.InBuild.Fire.performed += OnFirePerformed;
            currentPlayer.playerInput.InBuild.Fire.canceled += OnFireCanceled;

            currentPlayer.playerInput.InBuild.Look.performed += OnLookPerformed;
            currentPlayer.playerInput.InBuild.Look.canceled += OnLookCanceled;

            currentPlayer.playerInput.InBuild.Enable();

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
            RemovePlayer();
        }

        private void RemovePlayer()
        {
            if (!currentPlayer) return;
            currentPlayer.playerInput.InBuild.Disable();

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
            currentPlayer.AddPlayerState(PlayerCapabilityType.GunVisibility);
            currentPlayer.AddPlayerState(PlayerCapabilityType.Target);

            currentPlayer.SetPosition(playerPosition);
            currentPlayer.playerTransform.rotation = playerRotation;

            currentPlayer.PlayerCamera.CameraCollider.enabled = true;

            currentPlayer.PlayerCamera.ResetCameraTarget();

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
