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
        [System.NonSerialized]
        public ReadOnlyArray<InputDevice> devices;

        [System.NonSerialized]
        public int playerIndex;

        public OPPlayerInput playerInput;

        private List<PlayerCapability> playerStates = new List<PlayerCapability>();

        public PlayerMovement playerMovement;
        public PlayerShooting playerShooting;
        public PlayerHealth playerHealth;
        public PlayerInteraction playerInteraction;
        public EnemyTarget enemyTarget;
        public PlayerBuilding playerBuilding;
        public InvalidPlacement invalidPlacement;

        public Renderer[] playerRenderers;
        public Collider[] playerColliders;

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

            playerMovement = GetComponent<PlayerMovement>();
            playerShooting = GetComponent<PlayerShooting>();
            playerHealth = GetComponent<PlayerHealth>();
            playerInteraction = GetComponent<PlayerInteraction>();
            enemyTarget = GetComponent<EnemyTarget>();
            playerBuilding = GetComponent<PlayerBuilding>();
            invalidPlacement = GetComponent<InvalidPlacement>();

            playerRenderers = transform.GetChild(0).GetComponentsInChildren<Renderer>();
            playerColliders = transform.GetChild(1).GetComponentsInChildren<Collider>();
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
    }
}
