using OperationPlayground.UI;
using RicTools.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public class PlayerHealth : ObjectHealth
    {
        [SerializeField, MinValue(0.001f)]
        private float maxHealth;

        public override float MaxHealth => maxHealth;

        public override bool IsPlayer => true;

        private void Awake()
        {
            healthBarVisiblity = false;
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void OnDeath()
        {

        }
    }
}
