using OperationPlayground.Enemy;
using OperationPlayground.Managers;
using RicTools.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public class TowerHealth : ObjectHealth
    {
        [SerializeField, MinValue(1), ReadOnly(AvailableMode.Play)]
        private int maxHealth;

        public override int MaxHealth => maxHealth;

        protected override bool DoSpawnHealthBar => false;

        private void Awake()
        {
            GameManager.Instance.towerHealth = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                OnDeath();
            }
        }

        protected override void OnDeath()
        {
            EnemyRoundManager.Instance.StopRounds();
            GameManager.Instance.loseWinUI.ShowWin();
        }
    }
}
