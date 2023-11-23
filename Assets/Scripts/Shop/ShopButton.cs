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
        public ShopItemScriptableObject shopItem;

        public Image itemBackground;
        public Image itemImage;
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI itemCost;
        public Color normalColor;

        public void AssignShopItem(ShopItemScriptableObject newShopItem)
        {
            shopItem = newShopItem;
            itemImage.sprite = shopItem.sprite;
            itemName.text = shopItem.id;
            itemCost.text = shopItem.supplyCost.ToString();
        }

        public void SetButtonSelected()
        {
            Color selectColor = normalColor;
            selectColor.r *= 0.65f;
            selectColor.g *= 0.65f;
            selectColor.b *= 0.65f;
            itemBackground.color = selectColor;
        }

        public void SetButtonDeselected()
        {
            itemBackground.color = normalColor;
        }

        public void OnClick()
        {
            switch (shopItem.type)
            {
                case ShopItemWeaponType.Weapon:
                    {

                    }
                    break;
            }
            Debug.Log(shopItem.supplyCost + " Supplies deducted!");
        }
    }
}
