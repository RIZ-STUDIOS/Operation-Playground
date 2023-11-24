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
        private RoundManager roundManager;
        private DefendPointData defendPointData;

        private void Start()
        {
            roundManager = RoundManager.Instance;
            roundManager.onAllRoundsFinish += OnGameWon;

            defendPointData = GameManager.Instance.defendPointData;

            defendPointData.DefendPointHealth.onDeath += OnGameLost;
        }

        private void OnGameWon()
        {
            Debug.Log("Game Won");
            DisablePlayers();
            foreach(var playerManager in PlayerSpawnManager.Instance.Players)
            {
                playerManager.PlayerCanvas.gameOverUI.ShowWin();
            }
        }

        public void OnGameLost()
        {
            Debug.Log("Game Lost");
            roundManager.StopRounds();
            DisablePlayers();
            foreach (var playerManager in PlayerSpawnManager.Instance.Players)
            {
                playerManager.PlayerCanvas.gameOverUI.ShowLost();
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
