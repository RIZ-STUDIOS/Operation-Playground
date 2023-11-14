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

        private PlayerCanvas pC;
        private CanvasGroup cG;

        private void Awake()
        {
            pC = GetComponentInParent<PlayerCanvas>();
            cG = GetComponent<CanvasGroup>();
        }

        public void OpenShop(ShopItemScriptableObject[] shopItems)
        {
            pC.pM.RemovePlayerState(PlayerCapabilityType.Movement);
            pC.pM.RemovePlayerState(PlayerCapabilityType.Interaction);
            pC.pM.RemovePlayerState(PlayerCapabilityType.Building);
            pC.pM.RemovePlayerState(PlayerCapabilityType.Shooter);
            pC.pM.RemovePlayerState(PlayerCapabilityType.Health);
            pC.pM.RemovePlayerState(PlayerCapabilityType.InvalidPlacement);
            pC.pM.RemovePlayerState(PlayerCapabilityType.ToggleBuilding);

            StartCoroutine(pC.ToggleCanvasElement(cG, true, true));

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

            pC.pM.playerInput.UI.Cancel.performed += CloseShop;
        }

        public void CloseShop(InputAction.CallbackContext value)
        {
            foreach (Transform child in scrollShop.transform)
            {
                Destroy(child.gameObject);
            }

            StartCoroutine(pC.ToggleCanvasElement(cG, false, true));

            pC.pM.AddPlayerState(PlayerCapabilityType.Movement);
            pC.pM.AddPlayerState(PlayerCapabilityType.Interaction);
            pC.pM.AddPlayerState(PlayerCapabilityType.Building);
            pC.pM.AddPlayerState(PlayerCapabilityType.Shooter);
            pC.pM.AddPlayerState(PlayerCapabilityType.Health);
            pC.pM.AddPlayerState(PlayerCapabilityType.InvalidPlacement);
            pC.pM.AddPlayerState(PlayerCapabilityType.ToggleBuilding);
        }
    }
}
