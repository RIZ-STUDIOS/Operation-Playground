using OperationPlayground.EntityData;
using OperationPlayground.Managers;
using OperationPlayground.Rounds;
using RicTools.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player
{
    public class PlayerHealth : GenericHealth
    {
        [SerializeField, MinValue(1)]
        private int maxHealth = 10;

        public override int MaxHealth => maxHealth;

        protected override bool DestroyOnDeath => false;

        private PlayerManager playerManager;

        protected override Vector3 HealthBarSpawnOffset => new Vector3(0, 1, 0);

        protected override void Awake()
        {
            base.Awake();
            playerManager = GetComponent<PlayerManager>();
            onDeath += OnDeath;
        }

        private void Start()
        {
            RoundManager.Instance.onRoundEnd += () => FullyHeal();
        }

        private void OnDeath()
        {
            if (!GameManager.Instance.playerRespawnManager) return;

            GameManager.Instance.playerRespawnManager.StartRespawnPlayer(playerManager);
        }
    }
}
