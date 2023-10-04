using OperationPlayground.Enemy;
using OperationPlayground.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace OperationPlayground
{
    public class NextRoundUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI nextRoundText;

        private void Start()
        {
            EnemyRoundManager.Instance.onCountdownStart += UpdateNextEnemies;
        }

        private void UpdateNextEnemies()
        {
            var nextRound = EnemyRoundManager.Instance.GetNextRound();
            if(nextRound == null)
            {
                nextRoundText.text = "End";
                return;
            }

            nextRoundText.text = RoundEnemiesToString(nextRound);
        }

        private string RoundEnemiesToString(EnemyRoundScriptableObject data)
        {
            List<string> enemyString = new List<string>();

            foreach (var enemy in data.enemies)
            {
                enemyString.Add($"{enemy.count} {enemy.enemy.id}");
            }

            foreach (var enemy in data.supportEnemies)
            {
                enemyString.Add($"{enemy.count} {enemy.enemy.id}");
            }

            return string.Join("\n", enemyString);
        }
    }
}
