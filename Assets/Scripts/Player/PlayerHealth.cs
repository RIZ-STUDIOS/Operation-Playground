using OperationPlayground.EntityData;
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

        protected override void Awake()
        {
            base.Awake();
        }
    }
}
