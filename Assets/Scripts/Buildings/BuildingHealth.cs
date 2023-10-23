using OperationPlayground.Interactables;
using OperationPlayground.Managers;
using OperationPlayground.ScriptableObjects;
using OperationPlayground.UI;
using RicTools.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Buildings
{
    public abstract class BuildingHealth : ObjectHealth
    {
        [System.NonSerialized]
        public BuildingScriptableObject buildingSo;

        public override int MaxHealth => buildingSo.health;

        protected override bool DoSpawnHealthBar => false;

        public override bool IsPlayer => true;

        private Interactable interactable;

        private EnemyTarget target;

        protected BuildingInteraction buildingInteraction;

        protected virtual void Awake()
        {
            interactable = GetComponent<Interactable>();
            if (interactable)
            {
                interactable.enabled = false;
            }
            target = GetComponent<EnemyTarget>();
            target.visible = false;
            buildingInteraction = gameObject.GetOrAddComponent<BuildingInteraction>();
        }

        public void Place()
        {
            if (interactable)
            {
                interactable.enabled = true;
            }

            target.visible = true;

            SpawnHealthBar();

            OnPlaced();
        }

        protected abstract void OnPlaced();

        protected override void OnDeath()
        {
            if (buildingInteraction)
            {
                buildingInteraction.OnExitBuilding();
            }
            Destroy(gameObject);
        }
    }
}
