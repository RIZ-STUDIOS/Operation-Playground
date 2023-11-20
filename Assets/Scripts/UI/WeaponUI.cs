using OperationPlayground.Player;
using OperationPlayground.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OperationPlayground.UI
{
    public class WeaponUI : MonoBehaviour
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
