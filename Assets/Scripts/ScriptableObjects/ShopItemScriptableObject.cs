using RicTools.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.ScriptableObjects
{
    public class ShopItemScriptableObject : GenericScriptableObject
    {
        public Sprite itemSprite;
        public int itemCost;
        public bool availableInTruckShop;
        public bool availableInRadioShop;
        public WeaponScriptableObject weaponSo;
    }
}
