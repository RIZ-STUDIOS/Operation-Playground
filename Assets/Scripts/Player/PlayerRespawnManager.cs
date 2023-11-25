using OperationPlayground.Managers;
using OperationPlayground.Player.PlayerCapabilities;
using OperationPlayground.Rounds;
using OperationPlayground.ZedExtensions;
using RicTools.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player
{
    public class PlayerRespawnManager : MonoBehaviour
    {
        private PlayerSpawnManager PlayerSpawnManager => PlayerSpawnManager.Instance;

        private List<Transform> takenSpawnLocations = new List<Transform>();

        public List<PlayerManager> deadPlayers = new List<PlayerManager>();

        private void Awake()
        {
            GameManager.Instance.playerRespawnManager = this;
        }

        private void Start()
        {
            RoundManager.Instance.onRoundEnd += OnPreRoundStart;
        }

        private void OnPreRoundStart()
        {
            foreach(var playerManager in deadPlayers)
            {
                SpawnPlayer(playerManager);
                playerManager.AddDefaultPlayerStates();
                playerManager.PlayerCanvas.DeathUI.InstantHideModule();
            }
            deadPlayers.Clear();
        }

        public void SpawnPlayers()
        {
            foreach (var player in PlayerSpawnManager.Players)
            {
                SpawnPlayer(player);
            }
        }

        public void SpawnPlayer(PlayerManager playerManager)
        {
            playerManager.Health.FullyHeal();
            Transform spawnLocation;
            do
            {
                spawnLocation = GameManager.Instance.gameLevelData.spawnLocations.GetRandomElement();
            } while (takenSpawnLocations.Contains(spawnLocation));
            playerManager.SetPosition(spawnLocation.position);
            takenSpawnLocations.Add(spawnLocation);
        }

        public void StartRespawnPlayer(PlayerManager playerManager)
        {
            playerManager.RemoveAllPlayerStates();
            playerManager.AddPlayerState(PlayerCapabilityType.Camera);

            if (RoundManager.Instance.RoundStatus == RoundStatus.Round)
            {
                deadPlayers.Add(playerManager);
                if(deadPlayers.Count == PlayerSpawnManager.TotalPlayers)
                {
                    GameStateManager.Instance.OnGameLost();
                }
            }
            else
            {
                SpawnPlayer(playerManager);
                playerManager.AddDefaultPlayerStates();
                playerManager.PlayerCanvas.ResetPlayerUI();
            }
        }

        private void Update()
        {
            if (takenSpawnLocations.Count > 0)
            {
                takenSpawnLocations.Clear();
            }
        }
    }
}
