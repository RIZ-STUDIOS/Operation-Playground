using OperationPlayground.EntityData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Buildings
{
    public class BuildingHealth : GenericHealth
    {
        private BuildingData _buildingData;

        public new BuildingData parentEntity
        {
            get
            {
                if (!_buildingData) _buildingData = base.parentEntity as BuildingData;
                return _buildingData;
            }
        }

        public override int MaxHealth => _buildingData.buildingScriptableObject.health;

        protected override void Awake()
        {

        }

        private void Start()
        {
            CreateHealthBar();
            Health = MaxHealth;
        }
    }
}
