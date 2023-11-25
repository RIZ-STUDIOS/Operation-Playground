using OperationPlayground.Buildings;
using OperationPlayground.EntityData;
using OperationPlayground.Player.PlayerCapabilities;
using OperationPlayground.Player.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player
{
    public class PlayerManager : GenericEntity
    {
        public Gamepad gamepad;

        public OPPlayerInput playerInput;

        [System.NonSerialized]
        public int playerIndex;

        public Transform playerTransform;

        public PlayerCamera PlayerCamera => this.GetIfNull(ref _playerCamera);

        public Renderer[] PlayerRenderers => this.GetIfNull(ref _playerRenderers);

        public Collider[] PlayerColliders => this.GetIfNull(ref _playerColliders);

        public PlayerMovement PlayerMovement => this.GetIfNull(ref _playerMovement);

        public PlayerLookTPS PlayerMovementTPS => this.GetIfNull(ref _playerMovementTPS);

        public PlayerShooter PlayerShooter => this.GetIfNull(ref _playerShooter);

        public PlayerHealth PlayerHealth => this.GetIfNull(ref _playerHealth);

        public PlayerInteraction PlayerInteraction => this.GetIfNull(ref _playerInteraction);

        public InvalidPlacement InvaidPlacement => this.GetIfNull(ref _invalidPlacement);

        public PlayerBuilding PlayerBuilding => this.GetIfNull(ref _playerBuilding);

        public PlayerMap PlayerMap => this.GetIfNull(ref _playerMap);

        public PlayerCanvasManager PlayerCanvas => this.GetIfNull(ref _playerCanvas);

        public CharacterController CharacterController => this.GetIfNull(ref _characterController);

        public PlayerLookMap PlayerLookMap => this.GetIfNull(ref _playerLook);

        public MapHighlight MapHighlight => this.GetIfNull(ref _mapHighlight);

        public PlayerShopUI PlayerShopUI => this.GetIfNull(ref _playerShopUI);

        private PlayerCamera _playerCamera;
        private Renderer[] _playerRenderers;
        private Collider[] _playerColliders;

        private PlayerMovement _playerMovement;
        private PlayerLookTPS _playerMovementTPS;
        private PlayerShooter _playerShooter;
        private PlayerHealth _playerHealth;
        private PlayerInteraction _playerInteraction;
        private PlayerBuilding _playerBuilding;
        private InvalidPlacement _invalidPlacement;
        private PlayerMap _playerMap;
        private PlayerCanvasManager _playerCanvas;
        private CharacterController _characterController;
        private PlayerLookMap _playerLook;
        private MapHighlight _mapHighlight;
        private PlayerShopUI _playerShopUI;

        public override GameTeam Team => GameTeam.TeamA;

        public override GenericHealth Health => PlayerHealth;
        public override GenericShooter Shooter => PlayerShooter;

        private List<PlayerCapability> playerStates = new List<PlayerCapability>();

        protected override void Awake()
        {
            InitializePlayer();
        }

        public void InitializePlayer()
        {
            if (playerInput != null) return;
            playerInput = new OPPlayerInput();
            playerInput.Enable();

            SetParentEntity();
        }

        public void AddPlayerState(PlayerCapabilityType playerStateType)
        {
            if (HasPlayerState(playerStateType)) return;

            var state = PlayerCapability.CreatePlayerCapability(playerStateType);

            state.playerManager = this;

            playerStates.Add(state);

            state.OnStateEnter();
        }

        public bool RemovePlayerState(PlayerCapabilityType playerStateType)
        {
            if (!HasPlayerState(playerStateType)) return false;

            var state = GetPlayerState(playerStateType);

            playerStates.Remove(state);

            state.OnStateLeave();

            return true;

        }

        public void AddDefaultPlayerStates()
        {
            AddPlayerState(PlayerCapabilityType.Camera);
            AddPlayerState(PlayerCapabilityType.Graphics);
            AddPlayerState(PlayerCapabilityType.Collision);
            AddPlayerState(PlayerCapabilityType.Movement);
            AddPlayerState(PlayerCapabilityType.Shooter);
            AddPlayerState(PlayerCapabilityType.ToggleBuilding);
            AddPlayerState(PlayerCapabilityType.Health);
            AddPlayerState(PlayerCapabilityType.Interaction);
            AddPlayerState(PlayerCapabilityType.InvalidPlacement);
            AddPlayerState(PlayerCapabilityType.MapViewInput);
            AddPlayerState(PlayerCapabilityType.TPSLook);
            AddPlayerState(PlayerCapabilityType.GunVisibility);
            AddPlayerState(PlayerCapabilityType.Target);
        }

        public void AddAllPlayerStates()
        {
            var values = System.Enum.GetValues(typeof(PlayerCapabilityType));
            for (int i = 1; i < values.Length; i++)
            {
                var value = values.GetValue(i);
                AddPlayerState((PlayerCapabilityType)value);
            }
        }

        private PlayerCapability GetPlayerState(PlayerCapabilityType playerStateType)
        {
            return playerStates.Find(p => p.CapabilityType == playerStateType);
        }

        public bool HasPlayerState(PlayerCapabilityType playerStateType)
        {
            return GetPlayerState(playerStateType) != null;
        }

        public void RemoveAllPlayerStates()
        {
            while (playerStates.Count > 0)
            {
                RemovePlayerState(playerStates[0].CapabilityType);
            }
        }

        public void SetPosition(Vector3 position)
        {
            var enabled = CharacterController.enabled;
            CharacterController.enabled = false;
            transform.position = position;
            CharacterController.enabled = enabled;
        }

        public void SetLayer(int layer)
        {
            PlayerCamera.Camera.cullingMask |= 1 << layer;

            MapHighlight.visibleLayerMask |= 1 << layer;

            SetLayer(PlayerCamera.Camera.gameObject, layer);
        }

        private void SetLayer(GameObject gameObject, int layer)
        {
            gameObject.layer = layer;

            foreach (Transform transform in gameObject.transform)
            {
                SetLayer(transform.gameObject, layer);
            }
        }
    }
}
