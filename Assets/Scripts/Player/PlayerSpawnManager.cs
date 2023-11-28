using OperationPlayground.Loading;
using OperationPlayground.Managers;
using OperationPlayground.Player.UI.Modules;
using OperationPlayground.Rounds;
using OperationPlayground.ZedExtensions;
using RicTools.Managers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player
{
    [DisallowMultipleComponent]
    public class PlayerSpawnManager : GenericManager<PlayerSpawnManager>
    {
        public List<PlayerManager> Players => _players;
        private List<PlayerManager> _players = new List<PlayerManager>();

        public event System.Action<PlayerManager> OnPlayerJoin;
        public event System.Action<PlayerManager> OnPlayerLeave;

        public bool AnyPlayersJoined => _players.Count > 0;
        public int TotalPlayers => _players.Count;

        public bool autoSetupPlayers;

        [SerializeField]
        private Color[] mapHighlightColors;

        public PlayerInputManager PlayerInputManager => this.GetIfNull(ref _playerInputManager);

        private PlayerInputManager _playerInputManager;

        protected override void Awake()
        {
            base.Awake();
            if (Instance == this)
                DontDestroyOnLoad(gameObject);
            else
                Destroy(GetComponent<PlayerInputManager>());

            Cursor.visible = false;
        }

        private void OnPlayerJoined(PlayerInput playerInput)
        {
            Debug.Log($"Player {playerInput.playerIndex} has joined the session!");

            DontDestroyOnLoad(playerInput.gameObject);

            var playerManager = playerInput.GetComponent<PlayerManager>();
            playerManager.playerIndex = playerInput.playerIndex;
            playerManager.InitializePlayer();
            playerManager.playerInput.devices = playerInput.devices;
            playerManager.gamepad = playerInput.GetDevice<Gamepad>();

            playerManager.SetLayer(LayerMask.NameToLayer($"Player{playerManager.playerIndex}"));

            playerManager.AddAllPlayerStates();
            playerManager.RemoveAllPlayerStates();

            playerManager.MapHighlight.HighLightColor = mapHighlightColors[playerManager.playerIndex];

            _players.Add(playerManager);

            OnPlayerJoin?.Invoke(playerManager);

            if (autoSetupPlayers) SetupPlayer(playerManager);
        }

        private void OnPlayerLeft(PlayerInput playerInput)
        {
            Debug.Log($"Player {playerInput.playerIndex} has left the session!");

            var playerManager = playerInput.GetComponent<PlayerManager>();

            _players.Remove(playerManager);

            OnPlayerLeave?.Invoke(playerManager);
        }

        public void StartGame()
        {
            DisableJoining();
            LoadIntoLevel();
        }

        public void LoadIntoLevel()
        {
            LevelLoader.LoadScene("MapDraft", OnGameSceneLoad);
        }

        public void SetupPlayers()
        {
            foreach (var player in _players)
            {
                SetupPlayer(player);
            }
        }

        private void SetupPlayer(PlayerManager playerManager)
        {
            playerManager.AddDefaultPlayerStates();
            playerManager.Health.FullyHeal();
            playerManager.PlayerCanvas.ConfigureModules();
            playerManager.PlayerShooter.ResetPlayerWeapons();
            RoundManager.Instance.onRoundEnd += () => playerManager.Health.FullyHeal();
            GameManager.Instance.playerRespawnManager.SpawnPlayer(playerManager);
        }

        private void OnGameSceneLoad()
        {
            GameManager.Instance.playerRespawnManager.SpawnPlayers();
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

        [ContextMenu("Return To Main Menu")]
        public void ReturnToMainMenu()
        {
            DisableJoining();
            foreach (var player in _players)
            {
                player.PlayerCanvas.ResetPlayerUI();
                player.RemoveAllPlayerStates();
            }
            LevelLoader.LoadScene("MainMenu");
        }
    }
}
