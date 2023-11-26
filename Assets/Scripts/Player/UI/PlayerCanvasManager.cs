using OperationPlayground.Player.UI.Modules;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.UI
{
    public class PlayerCanvasManager : MonoBehaviour
    {
        public PlayerManager playerManager;

        public ShopUIModule ShopUI => this.GetIfNull(ref _shopUI);

        public InteractUIModule InteractUI => this.GetIfNull(ref _interactUI);

        public MessageUIModule MessageUI => this.GetIfNull(ref _messageUI);

        public ReticleUIModule ReticleUI => this.GetIfNull(ref _reticleUI);

        public BuildingUIModule BuildingUI => this.GetIfNull(ref _buildingUI);

        public SupplyCountUIModule SupplyCountUI => this.GetIfNull(ref _supplyCountUI);

        public WeaponUIModule WeaponUI => this.GetIfNull(ref _weaponUI);

        public WeaponAmmoUIModule WeaponAmmoUI => this.GetIfNull(ref _weaponAmmoUI);

        public DeathUIModule DeathUI => this.GetIfNull(ref _deathUI);

        public GameOverUIModule GameOverUI => this.GetIfNull(ref _gameOverUI);

        public OptionsUIModule OptionsUI => this.GetIfNull(ref _optionsUI);

        private ShopUIModule _shopUI;
        private InteractUIModule _interactUI;
        private MessageUIModule _messageUI;
        private ReticleUIModule _reticleUI;
        private BuildingUIModule _buildingUI;
        private SupplyCountUIModule _supplyCountUI;
        private WeaponUIModule _weaponUI;
        private WeaponAmmoUIModule _weaponAmmoUI;
        private DeathUIModule _deathUI;
        private GameOverUIModule _gameOverUI;
        private OptionsUIModule _optionsUI;

        public UIModule[] uiModules;

        private void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            uiModules = GetComponentsInChildren<UIModule>();
        }

        public void ConfigureModules()
        {
            ShopUI.ConfigureUI();
            InteractUI.ConfigureUI();
            MessageUI.ConfigureUI();
            ReticleUI.ConfigureUI();
            BuildingUI.ConfigureUI();
            SupplyCountUI.ConfigureUI();
            WeaponUI.ConfigureUI();
            WeaponAmmoUI.ConfigureUI();
            DeathUI.ConfigureUI();
            GameOverUI.ConfigureUI();
            OptionsUI.ConfigureUI();
        }

        public void ResetPlayerUI()
        {
            ShopUI.CloseMenu();
            OptionsUI.CloseMenu();
            InteractUI.InstantHideModule();
            MessageUI.InstantHideModule();
            BuildingUI.InstantHideModule();
            DeathUI.InstantHideModule();
            GameOverUI.InstantHideModule();
        }
    }
}
