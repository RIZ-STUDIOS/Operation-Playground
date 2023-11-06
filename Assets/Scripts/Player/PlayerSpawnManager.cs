using OperationPlayground.Loading;
using OperationPlayground.Managers;
using RicTools.Managers;
using RicTools.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player
{
    [DisallowMultipleComponent]
    public class PlayerSpawnManager : GenericManager<PlayerSpawnManager>
    {
        private List<PlayerManager> players = new List<PlayerManager>();

        public event System.Action<PlayerManager> onPlayerJoin;
        public event System.Action<PlayerManager> onPlayerLeave;

        public bool AnyPlayersJoined => players.Count > 0;

        protected override void Awake()
        {
            base.Awake();
            if (Instance == this)
                DontDestroyOnLoad(gameObject);
        }

        private void OnPlayerJoined(PlayerInput playerInput)
        {
            Debug.Log($"Player {playerInput.playerIndex} has joined the session!");

            DontDestroyOnLoad(playerInput.gameObject);

            var playerManager = playerInput.GetComponent<PlayerManager>();
            playerManager.InitializePlayer();
            playerManager.playerInput.devices = playerInput.devices;
            playerManager.gamepad = playerInput.GetDevice<Gamepad>();
            playerManager.playerIndex = playerInput.playerIndex;

            playerManager.AddAllPlayerStates();
            playerManager.RemoveAllPlayerStates();

            players.Add(playerManager);

            onPlayerJoin?.Invoke(playerManager);
        }

        private void OnPlayerLeft(PlayerInput playerInput)
        {
            Debug.Log($"Player {playerInput.playerIndex} has left the session!");

            var playerManager = playerInput.GetComponent<PlayerManager>();

            players.Remove(playerManager);

            onPlayerLeave?.Invoke(playerManager);
        }

        public void StartGame()
        {
            DisableJoining();
            LevelLoader.LoadScene("Game", OnGameSceneLoad);
        }

        private void OnGameSceneLoad()
        {
            List<Transform> takenSpawnLocations = new List<Transform>();
            foreach (var player in players)
            {
                player.AddDefaultPlayerStates();
                Transform spawnLocation;
                do
                {
                    spawnLocation = GameManager.Instance.gameLevelData.spawnLocations.GetRandomElement();
                } while (takenSpawnLocations.Contains(spawnLocation));
                player.SetPosition(spawnLocation.position);
                takenSpawnLocations.Add(spawnLocation);
            }
        }

        public void EnableJoining()
        {
            PlayerInputManager.instance.EnableJoining();
        }

        public void DisableJoining()
        {
            PlayerInputManager.instance.DisableJoining();
        }
    }
}
