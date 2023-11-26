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

        protected override void Awake()
        {
            base.Awake();
        }

        public override void ConfigureUI()
        {
            GameStateManager.Instance.OnGameOver += GameOverQuery;
        }

        public void GameOverQuery()
        {
            if (GameStateManager.Instance.IsVictory) ShowWin();
            else ShowLost();
        }

        public void ShowWin()
        {
            var color = wonColor;
            backgroundImage.color = color;

            wonLostText.text = "VICTORY FOR THE FINNS";
            Debug.Log("Victory for the Finns!");

            FadeRevealModule();
        }

        public void ShowLost()
        {
            var color = lostColor;
            backgroundImage.color = color;

            wonLostText.text = "DEFEATED BY THE SOVIETS";

            FadeRevealModule();
        }
    }
}
