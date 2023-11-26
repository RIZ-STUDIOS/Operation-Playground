using OperationPlayground.ZedExtensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace OperationPlayground.Player.UI.Modules
{
    public class MessageUIModule : UIModule
    {
        private Coroutine _messageCoroutine;

        public void DisplayMessage(string text)
        {
            if (_messageCoroutine != null) StopCoroutine(_messageCoroutine);

            _canvasGroup.alpha = 0;
            _canvasGroup.GetComponentInChildren<TextMeshProUGUI>().text = text;

            _messageCoroutine = StartCoroutine(_canvasGroup.FadeInThenOut());
        }
        public override void ConfigureUI()
        {

        }

    }
}
