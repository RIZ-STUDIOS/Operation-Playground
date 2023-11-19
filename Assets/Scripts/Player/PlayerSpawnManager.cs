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

        public event System.Action<PlayerManager> OnPlayerJoin;
        public event System.Action<PlayerManager> OnPlayerLeave;

        public bool AnyPlayersJoined => players.Count > 0;
        public int TotalPlayers => players.Count;

        public bool isGameScene;

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

            playerManager.SetLayer(LayerMask.NameToLayer($"Player{playerManager.playerIndex}"));

            playerManager.AddAllPlayerStates();
            playerManager.RemoveAllPlayerStates();

            players.Add(playerManager);

            OnPlayerJoin?.Invoke(playerManager);

            if (isGameScene) SetupPlayer(playerManager);
        }

        private void OnPlayerLeft(PlayerInput playerInput)
        {
            Debug.Log($"Player {playerInput.playerIndex} has left the session!");

            var playerManager = playerInput.GetComponent<PlayerManager>();

            players.Remove(playerManager);

            OnPlayerLeave?.Invoke(playerManager);
        }

        public void StartGame()
        {
            DisableJoining();
            LoadIntoLevel();
        }

        public void LoadIntoLevel()
        {
            LevelLoader.LoadScene("Game", OnGameSceneLoad);
        }

        public void SetupPlayers()
        {
            foreach(var player in players)
            {
                SetupPlayer(player);
            }
        }

        private void SetupPlayer(PlayerManager playerManager)
        {
            playerManager.AddDefaultPlayerStates();
        }

        private void OnGameSceneLoad()
        {
            List<Transform> takenSpawnLocations = new List<Transform>();
            foreach (var player in players)
            {
                Transform spawnLocation;
                do
                {
                    spawnLocation = GameManager.Instance.gameLevelData.spawnLocations.GetRandomElement();
                } while (takenSpawnLocations.Contains(spawnLocation));
                player.SetPosition(spawnLocation.position);
                takenSpawnLocations.Add(spawnLocation);
            }
            SetupPlayers();
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
