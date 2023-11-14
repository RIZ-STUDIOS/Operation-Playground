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

        public Image itemImage;
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI itemCost;

        public void AssignShopItem(ShopItemScriptableObject newShopItem)
        {
            shopItem = newShopItem;
            itemImage.sprite = shopItem.itemSprite;
            itemName.text = shopItem.itemName;
            itemCost.text = shopItem.itemCost.ToString();
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        public void OnClick()
        {
            Debug.Log(shopItem.itemCost + " Supplies deducted!");
        }
    }
}
