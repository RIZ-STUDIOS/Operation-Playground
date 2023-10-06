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

        public override bool IsPlayer => true;

        protected override void Start()
        {
            base.Start();
        }

        protected override void OnDeath()
        {

        }

        private void OnEnable()
        {
            if (!healthBarGameObject) return;
            healthBarGameObject.SetActive(true);
        }

        private void OnDisable()
        {
            healthBarGameObject.SetActive(false);
        }
    }
}
