using OperationPlayground.Interactables;
using OperationPlayground.Managers;
using OperationPlayground.ScriptableObjects;
using OperationPlayground.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Buildings
{
    public abstract class BuildingHealth : ObjectHealth
    {
        [System.NonSerialized]
        public BuildingScriptableObject buildingSo;

        [SerializeField]
        private Component[] scriptsToEnable;

        public override int MaxHealth => buildingSo.health;

        protected override bool DoSpawnHealthBar => false;

        public override bool IsPlayer => true;

        private Interactable interactable;

        private Collider[] colliders;

        private EnemyTarget target;

        private void Awake()
        {
            colliders = GetComponentsInChildren<Collider>();
            foreach (var collider in colliders)
            {
                collider.enabled = false;
            }

            interactable = GetComponent<Interactable>();
            if (interactable)
            {
                interactable.enabled = false;
            }
            target = GetComponent<EnemyTarget>();
            target.visible = false;
        }

        public void StartPlacement()
        {
            /*foreach (var script in scriptsToEnable)
            {
                var property = script.GetType().GetProperty("enabled");
                if (property != null)
                {
                    property.SetValue(script, false);
                }
            }*/
        }

        public void Place()
        {
            /*foreach (var script in scriptsToEnable)
            {
                var property = script.GetType().GetProperty("enabled");
                if (property != null)
                {
                    property.SetValue(script, true);
                }
            }*/

            if (interactable)
            {
                interactable.enabled = true;
            }

            foreach (var collider in colliders)
            {
                collider.enabled = true;
            }

            target.visible = true;

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
