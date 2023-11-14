using OperationPlayground.Shop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ShopItemSO", menuName = "1543493/ShopItem")]
    public class ShopItemScriptableObject : ScriptableObject
    {
        public Sprite itemSprite;
        public string itemName;
        public int itemCost;
        public ShopItemType itemType;
    }
}
