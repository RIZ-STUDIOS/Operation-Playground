using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OperationPlayground.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField]
        private Color wonColor;

        [SerializeField]
        private Color lostColor;

        [SerializeField]
        private Image backgroundImage;

        [SerializeField]
        private TextMeshProUGUI wonLostText;

        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void ShowWin()
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            var color = wonColor;
            color.a = 0.2f;
            backgroundImage.color = color;

            wonLostText.text = "The Fins Won the Battle";
        }

        public void ShowLost()
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            var color = lostColor;
            color.a = 0.2f;
            backgroundImage.color = color;

            wonLostText.text = "The Soviets Won the Battle";
        }

        public void HideCanvasGroup()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
        }
    }
}
