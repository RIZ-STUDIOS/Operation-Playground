using OperationPlayground.Managers;
using OperationPlayground.Player.PlayerCapabilities;
using OperationPlayground.Rounds;
using RicTools.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player
{
    public class PlayerRespawnManager : MonoBehaviour
    {
        [SerializeField]
        private float respawnTimer;

        private PlayerSpawnManager PlayerSpawnManager => PlayerSpawnManager.Instance;

        private List<Transform> takenSpawnLocations = new List<Transform>();

        private List<PlayerManager> deadPlayers = new List<PlayerManager>();

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
                playerManager.PlayerCanvas.HideDeathScreen();
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

        private void SpawnPlayer(PlayerManager playerManager)
        {
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
            playerManager.Health.FullyHeal();

            if (RoundManager.Instance.RoundStatus == RoundStatus.Round)
            {
                deadPlayers.Add(playerManager);
            }
            else
            {
                SpawnPlayer(playerManager);
                playerManager.AddDefaultPlayerStates();
                playerManager.PlayerCanvas.HideDeathScreen();
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
