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

        public override bool IsPlayer => true;

        private void Awake()
        {
            GameManager.Instance.towerHealth = this;
        }

        protected override void OnDeath()
        {
            GameManager.Instance.enemyRoundManager.StopRounds();
            GameManager.Instance.loseWinUI.ShowWin();
        }
    }
}
