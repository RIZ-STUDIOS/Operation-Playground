using OperationPlayground.EntityData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.DefendPoint
{
    public class DefendPointHealth : GenericHealth
    {
        [SerializeField]
        private int maxHealth = 200;

        [SerializeField]
        private Vector3 healthBarOffset;

        [SerializeField]
        private Vector3 healthBarSize;

        public override int MaxHealth => maxHealth;

        protected override Vector3 HealthBarSpawnOffset => healthBarOffset;

        protected override Vector3 HealthBarSize => healthBarSize;

        protected override void Awake()
        {
            base.Awake();
            ForceVisibleHealthBar = true;
            Destroy(healthBar.GetComponent<BillboardObject>());
        }
    }
}
