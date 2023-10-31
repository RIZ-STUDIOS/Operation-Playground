using OperationPlayground.Player.PlayerCapabilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public Gamepad gamepad;

        public OPPlayerInput playerInput;

        [System.NonSerialized]
        public int playerIndex;

        public Transform playerTransform;

        public PlayerCamera PlayerCamera => GetIfNull(ref _playerCamera);

        public Renderer[] PlayerRenderers => GetIfNull(ref _playerRenderers);

        public Collider[] PlayerColliders => GetIfNull(ref _playerColliders);

        public PlayerMovement PlayerMovement => GetIfNull(ref _playerMovement);

        private PlayerCamera _playerCamera;
        private Renderer[] _playerRenderers;
        private Collider[] _playerColliders;

        private PlayerMovement _playerMovement;

        private List<PlayerCapability> playerStates = new List<PlayerCapability>();

        private void Awake()
        {
            InitializePlayer();
        }

        public void InitializePlayer()
        {
            if (playerInput != null) return;
            playerInput = new OPPlayerInput();
            playerInput.Enable();
        }

        private T GetIfNull<T>(ref T component)
        {
            if (component == null)
                component = GetComponentInChildren<T>();

            return component;
        }

        private T[] GetIfNull<T>(ref T[] component)
        {
            if (component == null)
                component = GetComponentsInChildren<T>();

            return component;
        }

        public void AddPlayerState(PlayerCapabilityType playerStateType)
        {
            if (HasPlayerState(playerStateType)) return;

            var state = PlayerCapability.CreatePlayerCapability(playerStateType);

            state.playerManager = this;

            playerStates.Add(state);

            state.OnStateEnter();
        }

        public void RemovePlayerState(PlayerCapabilityType playerStateType)
        {
            if (!HasPlayerState(playerStateType)) return;

            var state = GetPlayerState(playerStateType);

            playerStates.Remove(state);

            state.OnStateLeave();

        }

        public void AddDefaultPlayerStates()
        {
            AddPlayerState(PlayerCapabilityType.Camera);
            AddPlayerState(PlayerCapabilityType.Graphics);
            AddPlayerState(PlayerCapabilityType.Collision);
            AddPlayerState(PlayerCapabilityType.Movement);
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
            PlayerMovement.SetPosition(position);
        }
    }
}
