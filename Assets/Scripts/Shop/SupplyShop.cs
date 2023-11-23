using OperationPlayground.Interactables;
using OperationPlayground.Player;
using OperationPlayground.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Shop
{
    public class SupplyShop : MonoBehaviour
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
            if (interactable.CanInteractWith) pM.GetComponentInChildren<PlayerShopUI>().OpenShop(shopItems);
            else pM.PlayerCanvas.DisplayPrompt("<color=#EC5D5D>SHOP UNAVAILABLE</color>");
        }
    }
}
