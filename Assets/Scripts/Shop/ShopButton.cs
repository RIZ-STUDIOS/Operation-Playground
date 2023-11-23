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
    public class ShopButton : MonoBehaviour
    {
        public ShopItemScriptableObject shopItem;

        public Image itemBackground;
        public Image itemIcon;
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI itemCost;
        public Color normalColor;
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

        public void BuyItem(PlayerManager playerManager)
        {
            if (ResourceManager.Instance.Supplies < ItemCost) return;
            ResourceManager.Instance.Supplies -= ItemCost;
            if (hasGun)
            {
                playerManager.PlayerShooter.GetWeaponBySO(shopItem.weaponSo).AddAmmo((int)(shopItem.weaponSo.maxAmmo * 0.5f));
            }
            else
            {
                playerManager.Shooter.AddWeapon(shopItem.weaponSo);
                hasGun = true;
                SetText();
            }
        }
    }
}
