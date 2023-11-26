using OperationPlayground.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace OperationPlayground.Player.UI.Modules
{
    public class WeaponUIModule : UIModule
    {
        [SerializeField]
        private Image weaponIconImage;

        protected override void Awake()
        {
            base.Awake();
            _playerCanvas.playerManager.PlayerShooter.onWeaponSwitch += onWeaponSwitch;
        }

        public override void ConfigureUI()
        {

        }

        private void onWeaponSwitch(Weapon weapon)
        {
            weaponIconImage.sprite = weapon.weaponSo.weaponSprite;
        }
    }
}
