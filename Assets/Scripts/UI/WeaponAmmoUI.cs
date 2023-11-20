using OperationPlayground.Player;
using OperationPlayground.Weapons;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace OperationPlayground.UI
{
    public class WeaponAmmoUI : MonoBehaviour
    {
        private PlayerManager playerManager;

        private Weapon currentWeapon;

        [SerializeField]
        private TextMeshProUGUI ammoText;

        private void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>();

            playerManager.PlayerShooter.onWeaponSwitch += (weapon) =>
            {
                if (currentWeapon)
                {
                    currentWeapon.onAmmoChange -= UpdateAmmoCount;
                }

                currentWeapon = weapon;

                currentWeapon.onAmmoChange += UpdateAmmoCount;
            };
        }

        private void UpdateAmmoCount()
        {
            if (currentWeapon.InfiniteAmmo)
            {
                ammoText.text = $"∞";
                return;
            }

            ammoText.text = $"{currentWeapon.CurrentAmmo}/{currentWeapon.weaponSo.maxAmmo}";
        }
    }
}
