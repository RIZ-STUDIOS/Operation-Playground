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

        private List<ShopButton> shopButtonList;

        private void Awake()
        {
            playerCanvas = GetComponentInParent<PlayerCanvas>();
            canvasGroup = GetComponent<CanvasGroup>();

            shopButtonList = new List<ShopButton>();
        }

        public void OpenShop(ShopItemScriptableObject[] shopItems)
        {
            foreach (var shopItem in shopItems)
            {
                if (SkipInvalidItem(shopItem)) continue;

                var buyButton = Instantiate(shopButtonPrefab);

                ShopButton shopButton = buyButton.GetComponent<ShopButton>();
                shopButton.AssignShopItem(shopItem);
                shopButton.transform.SetParent(scrollShop.transform, false);

                shopButtonList.Add(shopButton);
            }

            if (shopItems.Length <= 0)
            {
                playerCanvas.DisplayPrompt("<color=#EC5D5D>NO ITEMS IN SHOP</color>");
                return;
            }

            playerCanvas.playerManager.RemovePlayerState(PlayerCapabilityType.Movement);
            playerCanvas.playerManager.RemovePlayerState(PlayerCapabilityType.Interaction);
            playerCanvas.playerManager.RemovePlayerState(PlayerCapabilityType.Building);
            playerCanvas.playerManager.RemovePlayerState(PlayerCapabilityType.Shooter);
            playerCanvas.playerManager.RemovePlayerState(PlayerCapabilityType.ToggleBuilding);

            StartCoroutine(playerCanvas.ToggleCanvasElement(canvasGroup, true, true));

            shopButtonList[shopNavigationIndex].SetButtonSelected();

            EnableShopInput();
        }

        private bool SkipInvalidItem(ShopItemScriptableObject shopItem)
        {
            switch (shopItem.type)
            {
                case ShopItemWeaponType.Weapon:
                    {

                    }
                    break;
            }
            return false;
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
            int previousNavigationIndex = shopNavigationIndex;
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

                default:
                    return;
            }

            shopButtonList[previousNavigationIndex].SetButtonDeselected();
            shopButtonList[shopNavigationIndex].SetButtonSelected();
        }

        private void OnSubmit(InputAction.CallbackContext value)
        {
            shopButtonList[shopNavigationIndex].OnClick();
        }

        private void EnableShopInput()
        {
            playerCanvas.playerManager.playerInput.Basic.Disable();
            playerCanvas.playerManager.playerInput.UI.Cancel.performed += CloseShop;
            playerCanvas.playerManager.playerInput.UI.Navigate.performed += OnNavigate;
            playerCanvas.playerManager.playerInput.UI.Submit.performed += OnSubmit;
        }

        private void DisableShopInput()
        {
            playerCanvas.playerManager.playerInput.UI.Submit.performed -= OnSubmit;
            playerCanvas.playerManager.playerInput.UI.Navigate.performed -= OnNavigate;
            playerCanvas.playerManager.playerInput.UI.Cancel.performed -= CloseShop;
            playerCanvas.playerManager.playerInput.Basic.Enable();
        }
    }
}
