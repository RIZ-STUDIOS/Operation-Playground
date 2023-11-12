using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Menus
{
    public class MainMenu : GenericMenu
    {
        public static MainMenu Instance => _instance;

        public CanvasGroup mainMenu;
        public CanvasGroup lobbyMenu;
        public CanvasGroup settingsMenu;
        public CanvasGroup creditsMenu;

        private CanvasGroup activeCanvas;

        private static MainMenu _instance;

        private void Awake()
        {
            if (_instance)
            {
                Destroy(this);
                return;
            }
            _instance = this;
        }

        private void Update()
        {
            if (Gamepad.current.buttonEast.IsPressed() && activeCanvas != mainMenu)
            {
                if (activeCanvas == settingsMenu) HideSettings();
                else if (activeCanvas == creditsMenu) HideCredits();
            }
        }

        public void ShowLobby()
        {
            StartCoroutine(TransitionMenu(mainMenu, false, new Vector2(0, -1), 2));
            StartCoroutine(TransitionMenu(lobbyMenu, true, new Vector2(0, 1), 2));
            activeCanvas = lobbyMenu;
        }

        public void HideLobby()
        {
            StartCoroutine(TransitionMenu(lobbyMenu, false, new Vector2(0, 1), 2));
            StartCoroutine(TransitionMenu(mainMenu, true, new Vector2(0, -1), 2));
            activeCanvas = mainMenu;
        }

        public void ShowSettings()
        {
            StartCoroutine(TransitionMenu(mainMenu, false, new Vector2(1, 0), 2));
            StartCoroutine(TransitionMenu(settingsMenu, true, new Vector2(-1, 0), 2));
            activeCanvas = settingsMenu;
        }

        public void HideSettings()
        {
            StartCoroutine(TransitionMenu(settingsMenu, false, new Vector2(-1, 0), 2));
            StartCoroutine(TransitionMenu(mainMenu, true, new Vector2(1, 0), 2));
            activeCanvas = mainMenu;
        }

        public void ShowCredits()
        {
            StartCoroutine(TransitionMenu(mainMenu, false, new Vector2(-1, 0), 2));
            StartCoroutine(TransitionMenu(creditsMenu, true, new Vector2(1, 0), 2));
            activeCanvas = creditsMenu;
        }

        public void HideCredits()
        {
            StartCoroutine(TransitionMenu(creditsMenu, false, new Vector2(1, 0), 2));
            StartCoroutine(TransitionMenu(mainMenu, true, new Vector2(-1, 0), 2));
            activeCanvas = mainMenu;
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
