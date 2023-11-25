using OperationPlayground.Managers;
using OperationPlayground.Player;
using OperationPlayground.ZedExtensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OperationPlayground.Player.UI
{
    public class PlayerGameOverUI : MonoBehaviour
    {
        private PlayerCanvasManager _playerCanvas;
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private Color wonColor;

        [SerializeField]
        private Color lostColor;

        [SerializeField]
        private Image backgroundImage;

        [SerializeField]
        private TextMeshProUGUI wonLostText;

        private Coroutine _gameOverCoroutine;

        private void Awake()
        {
            _playerCanvas = GetComponentInParent<PlayerCanvasManager>();
            _canvasGroup = GetComponent<CanvasGroup>();

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
