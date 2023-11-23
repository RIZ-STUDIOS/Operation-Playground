using OperationPlayground.Player.PlayerCapabilities;
using OperationPlayground.ScriptableObjects;
using OperationPlayground.Shop;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace OperationPlayground.Player
{
    public class PlayerShopUI : MonoBehaviour
    {
        public GameObject shopButtonPrefab;
        public GameObject scrollShop;

        private PlayerCanvas playerCanvas;
        private CanvasGroup canvasGroup;

        private int shopNavigationIndex = 0;

        public List<Button> shopButtonList;

        private void Awake()
        {
            playerCanvas = GetComponentInParent<PlayerCanvas>();
            canvasGroup = GetComponent<CanvasGroup>();

            shopButtonList = new List<Button>();
        }

        public void OpenShop(ShopItemScriptableObject[] shopItems)
        {
            if (shopItems.Length <= 0)
            {
                playerCanvas.DisplayPrompt("<color=#EC5D5D>SHOP UNAVAILABLE</color>", 1f);
                return;
            }

            playerCanvas.playerManager.RemovePlayerState(PlayerCapabilityType.Movement);
            playerCanvas.playerManager.RemovePlayerState(PlayerCapabilityType.Interaction);
            playerCanvas.playerManager.RemovePlayerState(PlayerCapabilityType.Building);
            playerCanvas.playerManager.RemovePlayerState(PlayerCapabilityType.Shooter);
            playerCanvas.playerManager.RemovePlayerState(PlayerCapabilityType.ToggleBuilding);

            StartCoroutine(playerCanvas.ToggleCanvasElement(canvasGroup, true, true));

            foreach (var shopItem in shopItems)
            {
                var buyButton = Instantiate(shopButtonPrefab);
                buyButton.GetComponent<ShopButton>().AssignShopItem(shopItem);
                buyButton.transform.SetParent(scrollShop.transform, false);
                shopButtonList.Add(buyButton.GetComponent<Button>());
            }

            shopButtonList[shopNavigationIndex].Select();

            EnableShopInput();
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

            shopButtonList.Clear();

            DisableShopInput();
        }

        private void OnNavigate(InputAction.CallbackContext value)
        {
            Vector2 input = value.ReadValue<Vector2>();

            switch (input.y)
            {
                // Go up shop buttons.
                case 1:
                    {
                        shopNavigationIndex--;
                        if (shopNavigationIndex < 0) shopNavigationIndex = shopButtonList.Count - 1;
                    }
                    break;

                // Go down shop buttons.
                case -1:
                    {
                        shopNavigationIndex++;
                        if (shopNavigationIndex >= shopButtonList.Count) shopNavigationIndex = 0;
                    }
                    break;
            }

            shopButtonList[shopNavigationIndex].Select();
        }

        private void EnableShopInput()
        {
            playerCanvas.playerManager.playerInput.Basic.Disable();
            playerCanvas.playerManager.playerInput.UI.Cancel.performed += CloseShop;
            playerCanvas.playerManager.playerInput.UI.Navigate.performed += OnNavigate;
        }

        private void DisableShopInput()
        {
            playerCanvas.playerManager.playerInput.UI.Navigate.performed -= OnNavigate;
            playerCanvas.playerManager.playerInput.UI.Cancel.performed -= CloseShop;
            playerCanvas.playerManager.playerInput.Basic.Enable();
        }
    }
}
