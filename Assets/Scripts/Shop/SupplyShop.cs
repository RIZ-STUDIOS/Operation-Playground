using OperationPlayground.Interactables;
using OperationPlayground.Player;
using OperationPlayground.ScriptableObjects;
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
            pM.GetComponentInChildren<PlayerShopUI>().OpenShop(shopItems);
        }
    }
}
