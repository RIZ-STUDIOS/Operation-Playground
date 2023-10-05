using OperationPlayground.Managers;
using OperationPlayground.ScriptableObjects;
using OperationPlayground.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Buildings
{
    public abstract class Building : ObjectHealth
    {
        [System.NonSerialized]
        public BuildingScriptableObject buildingSo;

        [SerializeField]
        private Component[] scriptsToEnable;

        public override int MaxHealth => buildingSo.health;

        protected override bool DoSpawnHealthBar => false;

        public void StartPlacement()
        {
            foreach (var script in scriptsToEnable)
            {
                var property = script.GetType().GetProperty("enabled");
                if (property != null)
                {
                    property.SetValue(script, false);
                }
            }
        }

        public void Place()
        {
            foreach (var script in scriptsToEnable)
            {
                var property = script.GetType().GetProperty("enabled");
                if (property != null)
                {
                    property.SetValue(script, true);
                }
            }

            SpawnHealthBar();

            OnPlaced();
        }

        protected abstract void OnPlaced();

        protected override void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}
