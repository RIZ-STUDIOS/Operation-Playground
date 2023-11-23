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
            RoundManager.Instance.onPreRoundStart += () => interactable.enabled = true;

            RoundManager.Instance.onPreRoundEnd += () =>
            {
                foreach (var player in PlayerSpawnManager.Instance.Players)
                {
                    if (player.PlayerShopUI.InShop)
                        player.PlayerShopUI.CloseShop();
                }

                interactable.enabled = false;
            };
        }

        private void OnInteract(PlayerManager pM)
        {
            if (interactable.CanInteractWith) pM.PlayerShopUI.OpenShop(shopItems);
            else pM.PlayerCanvas.DisplayPrompt("<color=#EC5D5D>SHOP UNAVAILABLE</color>");
        }
    }
}
