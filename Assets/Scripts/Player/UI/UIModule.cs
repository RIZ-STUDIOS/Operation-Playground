using OperationPlayground.Player.UI;
using OperationPlayground.ZedExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIModule : MonoBehaviour
    {
        public PlayerCanvasManager PlayerCanvas { get { return _playerCanvas; } }
        protected PlayerCanvasManager _playerCanvas;

        public CanvasGroup CanvasGroup { get { return _canvasGroup; } }
        protected CanvasGroup _canvasGroup;

        protected Coroutine _fadeCoroutine;

        protected virtual void Awake()
        {
            _playerCanvas = GetComponentInParent<PlayerCanvasManager>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void InstantRevealModule()
        {
            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);

            CanvasGroup.InstantShow();
        }

        public void InstantHideModule()
        {
            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);

            CanvasGroup.InstantHide();
        }

        public void FadeRevealModule()
        {
            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);

            _fadeCoroutine = StartCoroutine(_canvasGroup.FadeIn());
        }

        public void FadeHideModule()
        {
            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);

            _fadeCoroutine = StartCoroutine(_canvasGroup.FadeOut());
        }
    }
}
