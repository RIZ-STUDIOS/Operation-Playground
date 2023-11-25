using OperationPlayground.Interactables;
using OperationPlayground.ZedExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.UI
{
    public class PlayerInteractUI : MonoBehaviour
    {
        private PlayerCanvasManager _playerCanvas;
        private CanvasGroup _canvasGroup;

        private Coroutine _fadeCoroutine;

        private void Awake()
        {
            _playerCanvas = GetComponentInParent<PlayerCanvasManager>();
            _canvasGroup = GetComponent<CanvasGroup>();

            _playerCanvas.playerManager.PlayerInteraction.onSetInteractable += OnSetInteractable;
        }

        private void OnSetInteractable(Interactable interactable)
        {
            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);

            if (interactable && interactable.enabled)
                _fadeCoroutine = StartCoroutine(_canvasGroup.FadeIn());
            else
                _fadeCoroutine = StartCoroutine(_canvasGroup.FadeOut());
        }

        public void EnableInteract()
        {
            StartCoroutine(_canvasGroup.FadeIn());
        }

        public void DisableInteract(PlayerManager playerManager)
        {
            StartCoroutine(_canvasGroup.FadeOut());
        }
    }
}
