using OperationPlayground.EntityData;
using OperationPlayground.Interactables;
using OperationPlayground.Player;
using OperationPlayground.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.SupplyDrop
{
    [RequireComponent(typeof(SupplyCrateHealth))]
    public class SupplyCrate : GenericEntity
    {
        [System.NonSerialized]
        public Interactable interactable;

        [System.NonSerialized]
        public int supplyAmount;

        [SerializeField]
        private GameObject[] parachuteGameObjects;

        public override GenericHealth Health => this.GetIfNull(ref _health);

        public override GenericShooter Shooter => null;

        private SupplyCrateHealth _health;

        public override GameTeam Team => GameTeam.TeamA;

        protected override void Awake()
        {
            base.Awake();
            interactable = GetComponent<Interactable>();

            interactable.onInteract += OnInteract;
        }

        public void OnLand()
        {
            interactable.enabled = true;
            targettable = true;

            foreach (var obj in parachuteGameObjects)
            {
                obj.SetActive(false);
            }
        }

        private void OnInteract(PlayerManager playerManager)
        {
            ResourceManager.Instance.Supplies += supplyAmount;

            foreach(var weapon in playerManager.PlayerShooter.HeldWeapons)
            {
                weapon.AddAmmo((int)Mathf.Ceil(weapon.weaponSo.maxAmmo / 2f));
            }

            Destroy(gameObject);
        }
    }
}
