using OperationPlayground.UI;
using RicTools.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public class PlayerHealth : ObjectHealth
    {
        [SerializeField, MinValue(1)]
        private int maxHealth;

        public override int MaxHealth => maxHealth;

        protected override void Start()
        {
            base.Start();
        }

        protected override void OnDeath()
        {

        }
    }
}
