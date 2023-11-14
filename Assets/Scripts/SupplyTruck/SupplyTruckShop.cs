using OperationPlayground.Buildings;
using OperationPlayground.Interactables;
using OperationPlayground.Player;
using OperationPlayground.Player.PlayerCapabilities;
using OperationPlayground.ScriptableObjects;
using OperationPlayground.Shop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.SupplyTruck
{
    public class SupplyTruckShop : MonoBehaviour
    {
        public ShopItemScriptableObject[] shopItems;

        private Interactable interactable;

        private void Awake()
        {
            interactable = GetComponent<Interactable>();
            interactable.onInteract += OnInteract;
        }

        private void OnInteract(PlayerManager pM)
        {
            pM.GetComponentInChildren<SupplyShopUI>().OpenShop(shopItems);
        }
    }
}
