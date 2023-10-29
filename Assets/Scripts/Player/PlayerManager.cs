using OperationPlayground.Buildings;
using OperationPlayground.Player;
using OperationPlayground.Player.PlayerCapabilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace OperationPlayground.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [NonSerialized]
        public ReadOnlyArray<InputDevice> devices;

        [NonSerialized]
        public int playerIndex;

        public OPPlayerInput playerInput;
        public Gamepad gamepad;

        private List<PlayerCapability> playerStates = new List<PlayerCapability>();

        [NonSerialized]
        public PlayerMovement playerMovement;

        [NonSerialized]
        public PlayerShooting playerShooting;

        [NonSerialized]
        public PlayerHealth playerHealth;

        [NonSerialized]
        public PlayerInteraction playerInteraction;

        [NonSerialized]
        public EnemyTarget enemyTarget;

        [NonSerialized]
        public PlayerBuilding playerBuilding;

        [NonSerialized]
        public InvalidPlacement invalidPlacement;

        [NonSerialized]
        public Renderer[] playerRenderers;

        [NonSerialized]
        public Collider[] playerColliders;

        public RumbleController rumbleController;

        private void Awake()
        {
            GetData();
        }

        private void Start()
        {
            playerInput.devices = devices;

            AddDefaultPlayerStates();
        }

        public void GetData()
        {
            if (playerInput != null) return;
            playerInput = new OPPlayerInput();
            playerInput.Enable();

            playerMovement = GetComponentInParent<PlayerMovement>();
            playerShooting = GetComponent<PlayerShooting>();
            playerHealth = GetComponent<PlayerHealth>();
            playerInteraction = GetComponent<PlayerInteraction>();
            enemyTarget = GetComponent<EnemyTarget>();
            playerBuilding = GetComponent<PlayerBuilding>();
            invalidPlacement = GetComponent<InvalidPlacement>();

            playerRenderers = transform.GetChild(0).GetComponentsInChildren<Renderer>();
            playerColliders = transform.GetChild(1).GetComponentsInChildren<Collider>();

            rumbleController = new RumbleController(this);
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
            AddPlayerState(PlayerCapabilityType.Movement);
            AddPlayerState(PlayerCapabilityType.HealthBar);
            AddPlayerState(PlayerCapabilityType.Looking);
            AddPlayerState(PlayerCapabilityType.Shooting);
            AddPlayerState(PlayerCapabilityType.Building);
            AddPlayerState(PlayerCapabilityType.InvalidPlacement);
            AddPlayerState(PlayerCapabilityType.EnemyTarget);
            AddPlayerState(PlayerCapabilityType.Interaction);
            AddPlayerState(PlayerCapabilityType.Graphics);
            AddPlayerState(PlayerCapabilityType.Collision);
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

        private void OnDisable()
        {
            rumbleController.StopRumble();
        }

        private void OnDestroy()
        {
            rumbleController.StopRumble();
        }
    }
}
