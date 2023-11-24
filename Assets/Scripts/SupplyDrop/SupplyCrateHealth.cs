using OperationPlayground.EntityData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.SupplyDrop
{
    public class SupplyCrateHealth : GenericHealth
    {
        [SerializeField]
        private int maxHealth = 3;

        public override int MaxHealth => maxHealth;
    }
}
