using OperationPlayground.Weapons;
using TMPro;
using UnityEngine;

namespace OperationPlayground.Player.UI.Modules
{
    public class WeaponAmmoUIModule : UIModule
    {
        private Weapon currentWeapon;

        [SerializeField]
        private TextMeshProUGUI ammoText;

        protected override void Awake()
        {
            base.Awake();

            _playerCanvas.playerManager.PlayerShooter.onWeaponSwitch += (weapon) =>
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

        public override void ConfigureUI()
        {

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
