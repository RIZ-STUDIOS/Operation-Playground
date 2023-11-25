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
        }

        public void ShowWin()
        {
            if (_gameOverCoroutine != null) StopCoroutine(_gameOverCoroutine);

            StartCoroutine(_canvasGroup.FadeIn(true));
            var color = wonColor;
            color.a = 0.2f;
            backgroundImage.color = color;

            wonLostText.text = "VICTORY FOR THE FINNS";
        }

        public void ShowLost()
        {
            if (_gameOverCoroutine != null) StopCoroutine(_gameOverCoroutine);

            StartCoroutine(_canvasGroup.FadeIn(true));
            var color = lostColor;
            color.a = 0.2f;
            backgroundImage.color = color;

            wonLostText.text = "DEFEATED BY THE SOVIETS";
        }

        public void HideGameOverScreen()
        {
            if (_gameOverCoroutine != null) StopCoroutine(_gameOverCoroutine);

            _gameOverCoroutine = StartCoroutine(_canvasGroup.FadeOut());
        }
    }
}
