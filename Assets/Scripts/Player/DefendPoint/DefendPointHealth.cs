using OperationPlayground.EntityData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.DefendPoint
{
    public class DefendPointHealth : GenericHealth
    {
        [SerializeField]
        private int health = 200;

        public override int MaxHealth => health;

        protected override Vector3 HealthBarSpawnOffset => new Vector3(0, 3, 0);

        protected override Vector3 HealthBarSize => base.HealthBarSize * 3f;

        protected override void Awake()
        {
            base.Awake();
            ForceVisibleHealthBar = true;
        }
    }
}
