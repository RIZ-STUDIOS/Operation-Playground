using OperationPlayground.Managers;
using OperationPlayground.ZedExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OperationPlayground.Player.UI.Modules
{
    public class GameOverUIModule : UIModule
    {
        [SerializeField]
        private Color wonColor;

        [SerializeField]
        private Color lostColor;

        [SerializeField]
        private Image backgroundImage;

        [SerializeField]
        private TextMeshProUGUI wonLostText;

        private Coroutine _gameOverCoroutine;

        protected override void Awake()
        {
            base.Awake();

            GameStateManager.Instance.OnGameOver += GameOverQuery;
        }

        private void GameOverQuery()
        {
            if (GameStateManager.Instance.IsVictory) ShowWin();
            else ShowLost();
        }

        public void ShowWin()
        {
            if (_gameOverCoroutine != null) StopCoroutine(_gameOverCoroutine);

            var color = wonColor;
            backgroundImage.color = color;

            wonLostText.text = "VICTORY FOR THE FINNS";

            _gameOverCoroutine = StartCoroutine(_canvasGroup.FadeIn(true));
        }

        public void ShowLost()
        {
            if (_gameOverCoroutine != null) StopCoroutine(_gameOverCoroutine);

            var color = lostColor;
            backgroundImage.color = color;

            wonLostText.text = "DEFEATED BY THE SOVIETS";

            _gameOverCoroutine = StartCoroutine(_canvasGroup.FadeIn(true));
        }

        public void HideGameOverScreen()
        {
            if (_gameOverCoroutine != null) StopCoroutine(_gameOverCoroutine);

            _gameOverCoroutine = StartCoroutine(_canvasGroup.FadeOut());
        }
    }
}
