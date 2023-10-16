using RicTools.Managers;
using RicTools.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player
{
    public class PlayerSpawnManager : GenericManager<PlayerSpawnManager>
    {
        [System.NonSerialized]
        public List<PlayerManager> players = new List<PlayerManager>();
        public event System.Action<PlayerManager> onPlayerJoined;

        [SerializeField]
        private Transform[] spawnLocations;

        private int respawnTimer = 5;

        private void OnPlayerJoined(PlayerInput input)
        {
            Debug.Log($"Player {input.playerIndex} has joined the session!");

            var playerManager = input.gameObject.GetOrAddComponent<PlayerManager>();
            playerManager.gamepad = input.GetDevice<Gamepad>();
            playerManager.GetData();
            GameObject player = input.gameObject;

            playerManager.rumbleController.DoRumble(0, 0.2f, 0.2f);

            playerManager.devices = input.devices;
            playerManager.playerIndex = input.playerIndex;

            players.Add(playerManager);
            playerManager.playerHealth.onDeath += () => { StartCoroutine(RespawnPlayer(playerManager)); };

            SpawnPlayer(playerManager);

            onPlayerJoined?.Invoke(playerManager);
        }

        private IEnumerator RespawnPlayer(PlayerManager playerManager)
        {
            playerManager.RemoveAllPlayerStates();

            float timer = 0;

            while (timer < respawnTimer)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            playerManager.AddDefaultPlayerStates();

            var ph = playerManager.playerHealth;
            ph.Heal(ph.MaxHealth);

            SpawnPlayer(playerManager);
        }

        private void SpawnPlayer(PlayerManager playerManager)
        {
            playerManager.playerMovement.SetPosition(spawnLocations[playerManager.playerIndex].position);
            //playerManager.transform.position = new Vector3(200, 0, 0);
            //playerManager.transform.position = spawnLocations[playerManager.playerIndex].position;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            foreach (var transform in spawnLocations)
            {
                Gizmos.DrawWireSphere(transform.position, 2);
            }
        }
    }
}
