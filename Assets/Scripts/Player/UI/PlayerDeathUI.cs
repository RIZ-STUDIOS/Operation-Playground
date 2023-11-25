using OperationPlayground.Managers;
using OperationPlayground.Rounds;
using OperationPlayground.ZedExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.UI
{
    public class PlayerDeathUI : MonoBehaviour
    {
        private PlayerCanvasManager _playerCanvas;
        private CanvasGroup _canvasGroup;

        private Coroutine _deathCoroutine;

        private void Awake()
        {
            _playerCanvas = GetComponentInParent<PlayerCanvasManager>();
            _canvasGroup = GetComponent<CanvasGroup>();

            _playerCanvas.playerManager.Health.OnDeath += ShowDeathScreen;
            _playerCanvas.playerManager.PlayerHealth.OnRespawn += HideDeathScreen;
            GameStateManager.Instance.OnGameOver += HideDeathScreen;
        }

        public void ShowDeathScreen()
        {
            if (_deathCoroutine != null) StopCoroutine(_deathCoroutine);

            _deathCoroutine = StartCoroutine(_canvasGroup.FadeIn());
        }

        public void HideDeathScreen()
        {
            if (_deathCoroutine != null) StopCoroutine(_deathCoroutine);
            
            _deathCoroutine = StartCoroutine(_canvasGroup.FadeOut());
        }
    }
}
