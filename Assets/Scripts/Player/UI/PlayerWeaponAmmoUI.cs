using OperationPlayground.Weapons;
using TMPro;
using UnityEngine;

namespace OperationPlayground.Player.UI
{
    public class PlayerWeaponAmmoUI : MonoBehaviour
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

                if (currentWeapon)
                {
                    currentWeapon.onAmmoChange += UpdateAmmoCount;
                    UpdateAmmoCount();
                }
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
