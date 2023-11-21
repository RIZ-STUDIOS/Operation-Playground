using OperationPlayground.Managers;
using OperationPlayground.Player.PlayerCapabilities;
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

        private void Awake()
        {
            GameManager.Instance.playerRespawnManager = this;
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

            StartCoroutine(PlayerRespawnCoroutine(playerManager));
        }

        private void Update()
        {
            if (takenSpawnLocations.Count > 0)
            {
                takenSpawnLocations.Clear();
            }
        }

        private IEnumerator PlayerRespawnCoroutine(PlayerManager playerManager)
        {
            float timer = 0;

            while (timer < respawnTimer)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            SpawnPlayer(playerManager);
            playerManager.AddDefaultPlayerStates();
        }
    }
}
