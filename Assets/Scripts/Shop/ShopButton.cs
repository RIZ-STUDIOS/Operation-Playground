using OperationPlayground.Player;
using OperationPlayground.Resources;
using OperationPlayground.ScriptableObjects;
using OperationPlayground.Weapons;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OperationPlayground.Shop
{
    public class ShopButton : ZedButton
    {
        public ShopItemScriptableObject shopItem;

        public Image itemIcon;
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI itemCost;
        public bool hasGun;

        private int ItemCost
        {
            get
            {
                if (!hasGun) return shopItem.supplyCost;
                return (int)(shopItem.supplyCost / 2f);
            }
        }

        public void AssignShopItem(ShopItemScriptableObject newShopItem, PlayerManager owningPlayer)
        {
            shopItem = newShopItem;
            itemIcon.sprite = shopItem.sprite;

            if (owningPlayer.PlayerShooter.GetWeaponBySO(newShopItem.weaponSo) != null) hasGun = true;

            Debug.Log(hasGun);

            SetText();
        }

        public void SetText()
        {
            if (hasGun)
            {
                itemName.text = shopItem.id + " Ammo";
                itemCost.text = ((int)(shopItem.supplyCost * 0.5f)).ToString();
            }
            else
            {
                itemName.text = shopItem.id;
                itemCost.text = shopItem.supplyCost.ToString();
            }
        }

        public override void OnSubmit(PlayerManager playerManager)
        {
            if (ResourceManager.Instance.Supplies < ItemCost)
            {
                playerManager.PlayerCanvas.DisplayPrompt("<color=#EC5D5D>INSUFFICIENT SUPPLIES</color>");
                return;
            }

            if (hasGun)
            {
                var weapon = playerManager.PlayerShooter.GetWeaponBySO(shopItem.weaponSo);
                if (weapon.AddAmmo((int)Mathf.Ceil(shopItem.weaponSo.maxAmmo * 0.5f)) <= 0)
                    ResourceManager.Instance.Supplies -= ItemCost;
            }
            else
            {
                ResourceManager.Instance.Supplies -= ItemCost;
                playerManager.Shooter.AddWeapon(shopItem.weaponSo);
                hasGun = true;
                SetText();
            }
        }
    }
}
