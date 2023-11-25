using OperationPlayground.ZedExtensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace OperationPlayground.Player.UI
{
    public class PlayerMessageUI : MonoBehaviour
    {
        private PlayerCanvasManager _playerCanvas;
        private CanvasGroup _canvasGroup;

        private Coroutine _messageCoroutine;

        private void Awake()
        {
            _playerCanvas = GetComponentInParent<PlayerCanvasManager>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void DisplayMessage(string text)
        {
            if (_messageCoroutine != null) StopCoroutine(_messageCoroutine);

            _canvasGroup.alpha = 0;
            _canvasGroup.GetComponentInChildren<TextMeshProUGUI>().text = text;

            _messageCoroutine = StartCoroutine(_canvasGroup.FadeInThenOut());
        }
    }
}
