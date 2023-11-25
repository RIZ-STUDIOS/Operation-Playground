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
        private bool _isGameOver;
        public bool IsGameOver { get { return _isGameOver; } private set { _isGameOver = value; } }

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
            DisablePlayers();
            foreach(var playerManager in PlayerSpawnManager.Instance.Players)
            {
                playerManager.PlayerCanvas.GameOverUI.ShowWin();
            }
        }

        public void OnGameLost()
        {
            Debug.Log("Game Lost");
            IsGameOver = true;
            roundManager.StopRounds();
            DisablePlayers();
            foreach (var playerManager in PlayerSpawnManager.Instance.Players)
            {
                playerManager.PlayerCanvas.GameOverUI.ShowLost();
            }
        }

        private void DisablePlayers()
        {
            foreach(var playerManager in PlayerSpawnManager.Instance.Players)
            {
                playerManager.RemoveAllPlayerStates();
                playerManager.AddPlayerState(PlayerCapabilityType.Camera);
                playerManager.AddPlayerState(PlayerCapabilityType.Graphics);
                playerManager.AddPlayerState(PlayerCapabilityType.Collision);
                playerManager.AddPlayerState(PlayerCapabilityType.Health);
                playerManager.AddPlayerState(PlayerCapabilityType.GunVisibility);
            }
        }
    }
}
