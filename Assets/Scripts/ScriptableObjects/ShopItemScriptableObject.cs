using OperationPlayground.Weapons;
using RicTools.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.ScriptableObjects
{
    public class ShopItemScriptableObject : GenericScriptableObject
    {
        public Sprite sprite;
        public int supplyCost;
        public ShopItemWeaponType type;
        public bool availableInTruckShop;
        public bool availableInRadioShop;
        public WeaponScriptableObject weaponSo;
    }

    public enum ShopItemWeaponType
    {
        None,
        Weapon
    }
}
