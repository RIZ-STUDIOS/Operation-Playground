using OperationPlayground.Interactables;
using OperationPlayground.Player;
using OperationPlayground.Rounds;
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

        private void Start()
        {
            RoundManager.Instance.onRoundEnd += () => interactable.enabled = true;

            RoundManager.Instance.onRoundStart += () =>
            {
                foreach (var player in PlayerSpawnManager.Instance.Players)
                {
                    if (player.PlayerShopUI.InShop)
                        player.PlayerShopUI.CloseShop();
                }

                interactable.enabled = false;
            };
        }

        private void OnInteract(PlayerManager playerManager)
        {
            if (interactable.CanInteractWith) playerManager.PlayerShopUI.OpenShop(shopItems);
            else playerManager.PlayerCanvas.MessageUI.DisplayMessage("<color=#EC5D5D>SHOP UNAVAILABLE</color>");
        }
    }
}
