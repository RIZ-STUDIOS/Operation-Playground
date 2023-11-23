using OperationPlayground.Interactables;
using OperationPlayground.Player;
using OperationPlayground.Rounds;
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

            RoundManager.Instance.onPreRoundStart += () => interactable.enabled = true;
            RoundManager.Instance.onPreRoundEnd += () =>
            {

                interactable.enabled = false;
            };
        }

        private void OnInteract(PlayerManager playerManager)
        {
            playerManager.PlayerShopUI.OpenShop(shopItems);
        }
    }
}
