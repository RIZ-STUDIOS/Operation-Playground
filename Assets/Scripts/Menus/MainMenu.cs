using UnityEngine;

namespace OperationPlayground.Menus
{
    public class MainMenu : GenericMenu
    {
        public static MainMenu Instance => _instance;

        public CanvasGroup mainMenu;
        public CanvasGroup lobbyMenu;
        public CanvasGroup settingsMenu;
        public CanvasGroup creditsMenu;

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

        public void ShowLobby()
        {
            StartCoroutine(TransitionMenu(mainMenu, false, new Vector2(0, -1), 2));
            StartCoroutine(TransitionMenu(lobbyMenu, true, new Vector2(0, 1), 2));
        }

        public void HideLobby()
        {
            StartCoroutine(TransitionMenu(lobbyMenu, false, new Vector2(0, 1), 2));
            StartCoroutine(TransitionMenu(mainMenu, true, new Vector2(0, -1), 2));
        }

        public void ShowSettings()
        {
            StartCoroutine(TransitionMenu(mainMenu, false, new Vector2(1, 0), 2));
            StartCoroutine(TransitionMenu(settingsMenu, true, new Vector2(-1, 0), 2));
        }

        public void HideSettings()
        {
            StartCoroutine(TransitionMenu(settingsMenu, false, new Vector2(-1, 0), 2));
            StartCoroutine(TransitionMenu(mainMenu, true, new Vector2(1, 0), 2));
        }

        public void ShowCredits()
        {
            StartCoroutine(TransitionMenu(mainMenu, false, new Vector2(-1, 0), 2));
            StartCoroutine(TransitionMenu(creditsMenu, true, new Vector2(1, 0), 2));
        }

        public void HideCredits()
        {
            StartCoroutine(TransitionMenu(creditsMenu, false, new Vector2(1, 0), 2));
            StartCoroutine(TransitionMenu(mainMenu, true, new Vector2(-1, 0), 2));
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
