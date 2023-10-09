using OperationPlayground.Buildings;
using OperationPlayground.Player;
using OperationPlayground.Player.PlayerStates;
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

        private List<PlayerState> playerStates = new List<PlayerState>();

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
            playerMovement = GetComponent<PlayerMovement>();
            playerShooting = GetComponent<PlayerShooting>();
            playerHealth = GetComponent<PlayerHealth>();
            playerInteraction = GetComponent<PlayerInteraction>();
            enemyTarget = GetComponent<EnemyTarget>();
            playerBuilding = GetComponent<PlayerBuilding>();
            invalidPlacement = GetComponent<InvalidPlacement>();

            playerRenderers = transform.GetChild(0).GetComponentsInChildren<Renderer>();
            playerColliders = transform.GetChild(1).GetComponentsInChildren<Collider>();

            playerInput = new OPPlayerInput();
            playerInput.Enable();
        }

        private void Start()
        {
            playerInput.devices = devices;

            AddPlayerState(PlayerStateType.Movement);
            AddPlayerState(PlayerStateType.HealthBar);
            AddPlayerState(PlayerStateType.Looking);
            AddPlayerState(PlayerStateType.Shooting);
            AddPlayerState(PlayerStateType.Building);
            AddPlayerState(PlayerStateType.InvalidPlacement);
            AddPlayerState(PlayerStateType.EnemyTarget);
            AddPlayerState(PlayerStateType.Interaction);
            AddPlayerState(PlayerStateType.Graphics);
            AddPlayerState(PlayerStateType.Collision);
        }

        public void AddPlayerState(PlayerStateType playerStateType)
        {
            if (HasPlayerState(playerStateType)) return;

            var state = PlayerState.CreatePlayerState(playerStateType);

            state.playerManager = this;

            playerStates.Add(state);

            state.OnStateEnter();
        }

        public void RemovePlayerState(PlayerStateType playerStateType)
        {
            if (!HasPlayerState(playerStateType)) return;

            var state = GetPlayerState(playerStateType);

            playerStates.Remove(state);

            state.OnStateLeave();

        }

        private PlayerState GetPlayerState(PlayerStateType playerStateType)
        {
            return playerStates.Find(p => p.StateType == playerStateType);
        }

        public bool HasPlayerState(PlayerStateType playerStateType)
        {
            return GetPlayerState(playerStateType) != null;
        }
    }
}
