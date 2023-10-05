using OperationPlayground.Enemy;
using RicTools.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OperationPlayground.UI
{
    public class CountdownUI : MonoBehaviour
    {
        [SerializeField, MustBeAssigned]
        private Image timerImage;

        private void Start()
        {
            EnemyRoundManager.Instance.onCountdownTick += DoCountdown;
            EnemyRoundManager.Instance.onRoundEnd += ShowCountdown;
            EnemyRoundManager.Instance.onRoundStart += HideCountdown;
        }

        private void DoCountdown(float timer)
        {
            timerImage.fillAmount = timer / EnemyRoundManager.Instance.TimeBetweenRounds;
        }

        private void ShowCountdown()
        {
            var color = timerImage.color;
            color.a = 1;
            timerImage.color = color;
        }

        private void HideCountdown()
        {
            var color = timerImage.color;
            color.a = 0;
            timerImage.color = color;
        }
    }
}
