using OperationPlayground.Managers;
using OperationPlayground.ScriptableObjects;
using OperationPlayground.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public abstract class Building : ObjectHealth
    {
        [System.NonSerialized]
        public BuildingScriptableObject buildingSo;

        [SerializeField]
        private Component[] scriptsToEnable;

        [SerializeField]
        private GameObject healthBarPrefab;

        public override int MaxHealth => buildingSo.health;

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
            foreach(var script in scriptsToEnable)
            {
                var property = script.GetType().GetProperty("enabled");
                if (property != null)
                {
                    property.SetValue(script, true);
                }
            }

            var healthBar = Instantiate(healthBarPrefab, transform);
            healthBar.transform.localPosition = new Vector3(0, 2.5f, 0);

            var enemyHealthUI = healthBar.GetComponentInChildren<ObjectHealthUI>();
            enemyHealthUI.parentHealth = this;

            OnPlaced();
        }

        protected abstract void OnPlaced();

        protected override void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}
