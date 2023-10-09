using OperationPlayground.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OperationPlayground.UI
{
    public class LoseWinUI : MonoBehaviour
    {
        [SerializeField]
        private Image backgroundImage;

        [SerializeField]
        private TextMeshProUGUI winLoseText;

        private CanvasGroup canvasGroup;

        [SerializeField]
        private Color winBackgroundColor;

        [SerializeField]
        private Color loseBackgroundColor;

        private void Awake()
        {
            GameManager.Instance.loseWinUI = this;
            canvasGroup = GetComponent<CanvasGroup>();
            Hide();
        }

        public void Hide()
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
        }

        public void ShowWin()
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1;
            backgroundImage.color = winBackgroundColor;
            winLoseText.text = "The Allies Won!";
        }

        public void ShowLose()
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1;
            backgroundImage.color = loseBackgroundColor;
            winLoseText.text = "The Axis Won!";
        }
    }
}
