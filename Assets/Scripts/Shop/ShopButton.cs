using OperationPlayground.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OperationPlayground.Shop
{
    public class ShopButton : MonoBehaviour
    {
        private const string COLOR_WEAPON_HEX = "#607661";
        private const string COLOR_ABILITY_HEX = "#6A6076";

        public ShopItemScriptableObject shopItem;

        public Image itemImage;
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI itemCost;

        public void AssignShopItem(ShopItemScriptableObject newShopItem)
        {
            Button button = GetComponent<Button>();
            Image buttonImage = button.GetComponent<Image>();

            shopItem = newShopItem;

            itemImage.sprite = shopItem.itemSprite;

            switch (shopItem.itemType)
            {
                case ShopItemType.Weapon:
                    {
                        ColorUtility.TryParseHtmlString(COLOR_WEAPON_HEX, out Color myColor);
                        buttonImage.color = myColor;
                    }
                    break;
                case ShopItemType.Ability:
                    {
                        ColorUtility.TryParseHtmlString(COLOR_ABILITY_HEX, out Color myColor);
                        buttonImage.color = myColor;
                    }
                    break;
            }

            itemName.text = shopItem.itemName;
            itemCost.text = shopItem.itemCost.ToString();
            button.onClick.AddListener(OnClick);
        }

        protected virtual void OnClick()
        {
            Debug.Log(shopItem.itemCost + " Supplies deducted!");
        }
    }

    public enum ShopItemType
    {
        None,
        Weapon,
        Ability
    }
}
