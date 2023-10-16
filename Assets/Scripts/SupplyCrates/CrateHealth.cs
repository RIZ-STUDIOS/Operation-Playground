using RicTools.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.SupplyCreates
{
    public class CrateHealth : ObjectHealth
    {
        [SerializeField]
        [Rename("Max Health")]
        [MinValue(1)]
        private int _maxHealth;

        public override int MaxHealth => _maxHealth;

        public override bool IsPlayer => true;

        protected override void OnDeath()
        {

        }
    }
}
