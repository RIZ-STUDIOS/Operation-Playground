using OperationPlayground.Interactables;
using OperationPlayground.Weapons;
using OperationPlayground.ZedExtensions;
using System.Collections;
using TMPro;
using UnityEngine;

namespace OperationPlayground.Player.UI
{
    public class PlayerCanvasManager : MonoBehaviour
    {
        public PlayerManager playerManager;

        public PlayerShopUI ShopUI => this.GetIfNull(ref _shopUI);

        public PlayerInteractUI InteractUI => this.GetIfNull(ref _interactUI);

        public PlayerMessageUI MessageUI => this.GetIfNull(ref _messageUI);

        public PlayerReticleUI ReticleUI => this.GetIfNull(ref _reticleUI);

        public PlayerBuildingUI BuildingUI => this.GetIfNull(ref _buildingUI);

        public PlayerWeaponUI WeaponUI => this.GetIfNull(ref _weaponUI);

        public PlayerWeaponAmmoUI WeaponAmmoUI => this.GetIfNull(ref _weaponAmmoUI);

        public PlayerDeathUI DeathUI => this.GetIfNull(ref _deathUI);

        public PlayerGameOverUI GameOverUI => this.GetIfNull(ref _gameOverUI);

        private PlayerShopUI _shopUI;
        private PlayerInteractUI _interactUI;
        private PlayerMessageUI _messageUI;
        private PlayerReticleUI _reticleUI;
        private PlayerBuildingUI _buildingUI;
        private PlayerDeathUI _deathUI;
        private PlayerWeaponAmmoUI _weaponAmmoUI;
        private PlayerGameOverUI _gameOverUI;
        private PlayerWeaponUI _weaponUI;

        private void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>();
        }
    }
}
