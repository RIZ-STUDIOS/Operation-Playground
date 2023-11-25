using OperationPlayground.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace OperationPlayground.Player.UI
{
    public class PlayerWeaponUI : MonoBehaviour
    {
        private PlayerManager playerManager;

        [SerializeField]
        private Image weaponIconImage;

        private void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>();

            playerManager.PlayerShooter.onWeaponSwitch += onWeaponSwitch;
        }

        private void onWeaponSwitch(Weapon weapon)
        {
            weaponIconImage.sprite = weapon.weaponSo.weaponSprite;
        }
    }
}
