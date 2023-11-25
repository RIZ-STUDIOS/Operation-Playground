using OperationPlayground.Player.PlayerCapabilities;
using OperationPlayground.ScriptableObjects;
using OperationPlayground.Shop;
using OperationPlayground.ZedExtensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player.UI
{
    public class PlayerShopUI : MonoBehaviour
    {
        private PlayerCanvasManager _playerCanvas;
        private CanvasGroup _canvasGroup;

        public GameObject shopButtonPrefab;
        public GameObject scrollShop;

        private int _shopNavigationIndex = 0;

        private List<ShopButton> _shopButtonList;

        public bool InShop => _inShop;

        private bool _inShop;

        private void Awake()
        {
            _playerCanvas = GetComponentInParent<PlayerCanvasManager>();
            _canvasGroup = GetComponent<CanvasGroup>();

            _shopButtonList = new List<ShopButton>();
        }

        public void OpenShop(ShopItemScriptableObject[] shopItems)
        {
            if (shopItems.Length <= 0)
            {
                _playerCanvas.MessageUI.DisplayMessage("<color=#EC5D5D>NO ITEMS IN SHOP</color>");
                return;
            }

            foreach (var shopItem in shopItems)
            {
                var buyButton = Instantiate(shopButtonPrefab);

                ShopButton shopButton = buyButton.GetComponent<ShopButton>();
                shopButton.AssignShopItem(shopItem, _playerCanvas.playerManager);
                shopButton.transform.SetParent(scrollShop.transform, false);

                _shopButtonList.Add(shopButton);
            }

            _playerCanvas.playerManager.RemovePlayerState(PlayerCapabilityType.Movement);
            _playerCanvas.playerManager.RemovePlayerState(PlayerCapabilityType.Interaction);
            _playerCanvas.playerManager.RemovePlayerState(PlayerCapabilityType.Building);
            _playerCanvas.playerManager.RemovePlayerState(PlayerCapabilityType.Shooter);
            _playerCanvas.playerManager.RemovePlayerState(PlayerCapabilityType.ToggleBuilding);

            StartCoroutine(_canvasGroup.FadeIn(true, true));

            _shopButtonList[_shopNavigationIndex].SetButtonSelected();

            EnableShopInput();

            _inShop = true;
        }

        public void OnCancelPerformed(InputAction.CallbackContext value)
        {
            CloseShop();
        }

        public void CloseShop()
        {
            foreach (Transform child in scrollShop.transform)
            {
                Destroy(child.gameObject);
            }

            StartCoroutine(_canvasGroup.FadeOut(true));

            _playerCanvas.playerManager.AddPlayerState(PlayerCapabilityType.Movement);
            _playerCanvas.playerManager.AddPlayerState(PlayerCapabilityType.Interaction);
            _playerCanvas.playerManager.AddPlayerState(PlayerCapabilityType.Shooter);
            _playerCanvas.playerManager.AddPlayerState(PlayerCapabilityType.ToggleBuilding);

            _shopButtonList.Clear();

            DisableShopInput();
            _inShop = false;
        }

        private void OnNavigate(InputAction.CallbackContext value)
        {
            int previousNavigationIndex = _shopNavigationIndex;
            Vector2 input = value.ReadValue<Vector2>();

            switch (input.y)
            {
                // Go up shop buttons.
                case 1:
                    {
                        _shopNavigationIndex--;
                        if (_shopNavigationIndex < 0) _shopNavigationIndex = _shopButtonList.Count - 1;
                    }
                    break;

                // Go down shop buttons.
                case -1:
                    {
                        _shopNavigationIndex++;
                        if (_shopNavigationIndex >= _shopButtonList.Count) _shopNavigationIndex = 0;
                    }
                    break;

                default:
                    return;
            }

            _shopButtonList[previousNavigationIndex].SetButtonDeselected();
            _shopButtonList[_shopNavigationIndex].SetButtonSelected();
        }

        private void OnSubmit(InputAction.CallbackContext value)
        {
            _shopButtonList[_shopNavigationIndex].OnSubmit(_playerCanvas.playerManager);
        }

        private void EnableShopInput()
        {
            _playerCanvas.playerManager.playerInput.Basic.Disable();
            _playerCanvas.playerManager.playerInput.UI.Cancel.performed += OnCancelPerformed;
            _playerCanvas.playerManager.playerInput.UI.Navigate.performed += OnNavigate;
            _playerCanvas.playerManager.playerInput.UI.Submit.performed += OnSubmit;
        }

        private void DisableShopInput()
        {
            _playerCanvas.playerManager.playerInput.UI.Submit.performed -= OnSubmit;
            _playerCanvas.playerManager.playerInput.UI.Navigate.performed -= OnNavigate;
            _playerCanvas.playerManager.playerInput.UI.Cancel.performed -= OnCancelPerformed;
            _playerCanvas.playerManager.playerInput.Basic.Enable();
        }
    }
}
