using OperationPlayground.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace OperationPlayground.Menus
{
    public class MainMenu : GenericMenu
    {
        public static MainMenu Instance => _instance;
        private static MainMenu _instance;

        public MainSubMenu ActiveMenu { get; private set; }
        public MainSubMenu mainMenu;
        public MainSubMenu lobbyMenu;
        public MainSubMenu settingsMenu;
        public MainSubMenu controlsMenu;
        public MainSubMenu creditsMenu;

        public Button lobbyButton;
        public Button settingsButton;
        public Button controlsButton;
        public Button creditsButton;
        public Button quitButton;

        private PlayerManager firstPlayer => PlayerSpawnManager.Instance.Players[0];
        private LobbyHandler lobby;

        private void Awake()
        {
            if (_instance)
            {
                Destroy(this);
                return;
            }
            _instance = this;
        }

        private void Start()
        {
            InitMainMenu();
        }

        private void InitMainMenu()
        {
            ActiveMenu = mainMenu;
            lobby = GetComponentInChildren<LobbyHandler>();
            lobby.onLobbyEnded += OnLobbyEnded;

            if (PlayerSpawnManager.Instance.Players.Count <= 0) WaitForFirstPlayer();
            else firstPlayer.playerInput.UI.Cancel.performed += OnCancel;

            lobbyButton.onClick.AddListener(OnLobbyButton);
            settingsButton.onClick.AddListener(OnSettingsButton);
            controlsButton.onClick.AddListener(OnControlsButton);
            creditsButton.onClick.AddListener(OnCreditsButton);
            quitButton.onClick.AddListener(OnQuitButton);
        }

        private void WaitForFirstPlayer()
        {
            PlayerSpawnManager.Instance.OnPlayerJoin += OnPlayerJoin;
            PlayerSpawnManager.Instance.EnableJoining();
            PlayerSpawnManager.Instance.PlayerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
        }

        private void FirstPlayerFound()
        {
            PlayerSpawnManager.Instance.OnPlayerJoin -= OnPlayerJoin;
            PlayerSpawnManager.Instance.DisableJoining();
            PlayerSpawnManager.Instance.PlayerInputManager.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenJoinActionIsTriggered;
        }

        private void OnPlayerJoin(PlayerManager player)
        {
            firstPlayer.playerInput.UI.Cancel.performed += OnCancel;
            FirstPlayerFound();
        }

        private void OnCancel(InputAction.CallbackContext input)
        {
            ReturnToMainMenu();
        }

        private void OnLobbyEnded()
        {
            firstPlayer.playerInput.UI.Cancel.performed += OnCancel;
            ReturnToMainMenu();
        }

        private void ReturnToMainMenu()
        {
            if (ActiveMenu == mainMenu) return;
            StartCoroutine(TransitionMenu(ActiveMenu.canvasGroup, false, ActiveMenu.entryDirection));
            StartCoroutine(TransitionMenu(mainMenu.canvasGroup, true, -ActiveMenu.entryDirection));
            ActiveMenu = mainMenu;
        }

        private void OnLobbyButton()
        {
            StartCoroutine(TransitionMenu(mainMenu.canvasGroup, false, -lobbyMenu.entryDirection));
            StartCoroutine(TransitionMenu(lobbyMenu.canvasGroup, true, lobbyMenu.entryDirection));
            firstPlayer.playerInput.UI.Cancel.performed -= OnCancel;
            ActiveMenu = lobbyMenu;
            lobby.StartLobby();
        }

        private void OnSettingsButton()
        {
            StartCoroutine(TransitionMenu(mainMenu.canvasGroup, false, -settingsMenu.entryDirection));
            StartCoroutine(TransitionMenu(settingsMenu.canvasGroup, true, settingsMenu.entryDirection));
            ActiveMenu = settingsMenu;
        }

        private void OnControlsButton()
        {
            StartCoroutine(TransitionMenu(mainMenu.canvasGroup, false, -controlsMenu.entryDirection));
            StartCoroutine(TransitionMenu(controlsMenu.canvasGroup, true, controlsMenu.entryDirection));
            ActiveMenu = controlsMenu;
        }

        private void OnCreditsButton()
        {
            StartCoroutine(TransitionMenu(mainMenu.canvasGroup, false, -creditsMenu.entryDirection));
            StartCoroutine(TransitionMenu(creditsMenu.canvasGroup, true, creditsMenu.entryDirection));
            ActiveMenu = creditsMenu;
        }

        public void OnQuitButton()
        {
            Application.Quit();
        }
    }
}
