using RicTools.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player
{
    public class PlayerSpawnManager : MonoBehaviour
    {
        [SerializeField]
        private Transform[] spawnLocations;
        public List<GameObject> players;

        private int respawnTimer = 5;

        private void Start()
        {
            players = new List<GameObject>();
        }

        private void OnPlayerJoined(PlayerInput input)
        {
            Debug.Log($"Player {input.playerIndex} has joined the session!");

            var playerInputData = input.gameObject.GetOrAddComponent<PlayerInputManager>();
            GameObject player = input.gameObject;

            playerInputData.devices = input.devices;
            playerInputData.playerIndex = input.playerIndex;

            players.Add(player);
            player.GetComponent<PlayerHealth>().onDeath += () => { StartCoroutine(RespawnPlayer(input.gameObject)); };

            SpawnPlayer(player);
        }

        private IEnumerator RespawnPlayer(GameObject player)
        {
            player.SetActive(false);

            float timer = 0;

            while (timer < respawnTimer)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            player.SetActive(true);

            var ph = player.GetComponent<PlayerHealth>();
            ph.Heal(ph.MaxHealth);

            SpawnPlayer(player);
        }

        private void SpawnPlayer(GameObject player)
        {
            player.transform.position = spawnLocations[Random.Range(0, spawnLocations.Length)].position;
        }
    }
}
