using OperationPlayground.Player;
using OperationPlayground.Player.DefendPoint;
using OperationPlayground.Player.PlayerCapabilities;
using OperationPlayground.Rounds;
using RicTools.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Managers
{
    public class GameStateManager : GenericManager<GameStateManager>
    {
        public event System.Action OnGameOver;

        public bool IsGameOver { get { return _isGameOver; } }
        private bool _isGameOver;

        public bool IsVictory { get { return _isVictory; } }
        private bool _isVictory;

        private RoundManager roundManager;
        private DefendPointData defendPointData;

        private void Start()
        {
            roundManager = RoundManager.Instance;
            roundManager.onAllRoundsFinish += OnGameWon;

            defendPointData = GameManager.Instance.defendPointData;

            defendPointData.DefendPointHealth.OnDeath += OnGameLost;
        }

        private void OnGameWon()
        {
            Debug.Log("Game Won");
            _isGameOver = true;
            _isVictory = true;
            OnGameOver?.Invoke();
            DisablePlayers();
        }

        public void OnGameLost()
        {
            Debug.Log("Game Lost");
            _isGameOver = true;
            _isVictory = false;
            roundManager.StopRounds();
            OnGameOver?.Invoke();
            DisablePlayers();
        }

        private void DisablePlayers()
        {
            foreach(var playerManager in PlayerSpawnManager.Instance.Players)
            {
                playerManager.PlayerInteraction.SetInteractable(null);
                playerManager.RemoveAllPlayerStates();
                playerManager.AddPlayerState(PlayerCapabilityType.Camera);
                playerManager.AddPlayerState(PlayerCapabilityType.Graphics);
                playerManager.AddPlayerState(PlayerCapabilityType.Collision);
                playerManager.AddPlayerState(PlayerCapabilityType.Health);
                playerManager.AddPlayerState(PlayerCapabilityType.GunVisibility);

                playerManager.PlayerCanvas.DeathUI.InstantHideModule();
            }
        }
    }
}
