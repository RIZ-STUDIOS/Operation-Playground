using OperationPlayground.Player;
using OperationPlayground.Player.PlayerCapabilities;
using OperationPlayground.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace OperationPlayground.Shop
{
    public class SupplyShopUI : MonoBehaviour
    {
        public GameObject shopButtonPrefab;
        public GameObject scrollShop;

        private PlayerCanvas playerCanvas;
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            playerCanvas = GetComponentInParent<PlayerCanvas>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OpenShop(ShopItemScriptableObject[] shopItems)
        {
            playerCanvas.playerManager.RemovePlayerState(PlayerCapabilityType.Movement);
            playerCanvas.playerManager.RemovePlayerState(PlayerCapabilityType.Interaction);
            playerCanvas.playerManager.RemovePlayerState(PlayerCapabilityType.Building);
            playerCanvas.playerManager.RemovePlayerState(PlayerCapabilityType.Shooter);
            playerCanvas.playerManager.RemovePlayerState(PlayerCapabilityType.ToggleBuilding);

            StartCoroutine(playerCanvas.ToggleCanvasElement(canvasGroup, true, true));

            if (shopItems.Length > 0)
            {
                foreach (var shopItem in shopItems)
                {
                    var buyButton = Instantiate(shopButtonPrefab);
                    buyButton.GetComponent<ShopButton>().AssignShopItem(shopItem);
                    buyButton.transform.SetParent(scrollShop.transform, false);
                }
            }

            if (scrollShop.transform.childCount > 0)
            {
                scrollShop.transform.GetChild(0).GetComponent<Button>().Select();
            }

            playerCanvas.playerManager.playerInput.Basic.Disable();
            playerCanvas.playerManager.playerInput.UI.Cancel.performed += CloseShop;
        }

        public void CloseShop(InputAction.CallbackContext value)
        {
            foreach (Transform child in scrollShop.transform)
            {
                Destroy(child.gameObject);
            }

            StartCoroutine(playerCanvas.ToggleCanvasElement(canvasGroup, false, true));

            playerCanvas.playerManager.AddPlayerState(PlayerCapabilityType.Movement);
            playerCanvas.playerManager.AddPlayerState(PlayerCapabilityType.Interaction);
            playerCanvas.playerManager.AddPlayerState(PlayerCapabilityType.Shooter);
            playerCanvas.playerManager.AddPlayerState(PlayerCapabilityType.ToggleBuilding);

            playerCanvas.playerManager.playerInput.Basic.Enable();
        }
    }
}
