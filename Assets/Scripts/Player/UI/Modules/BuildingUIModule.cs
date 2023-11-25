using OperationPlayground.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OperationPlayground.Player.UI.Modules
{
    public class BuildingUIModule : UIModule
    {
        [SerializeField]
        private Image buildingImage;

        [SerializeField]
        private TextMeshProUGUI supplyCountText;

        protected override void Awake()
        {
            base.Awake();

            _playerCanvas.playerManager.PlayerBuilding.onEnterBuildingMode += () => _canvasGroup.alpha = 1;
            _playerCanvas.playerManager.PlayerBuilding.onExitBuildingMode += () => _canvasGroup.alpha = 0;

            _playerCanvas.playerManager.PlayerBuilding.onBuildingSelected += OnBuildingSelected;
        }

        private void OnBuildingSelected(BuildingScriptableObject buildingSo)
        {
            buildingImage.sprite = buildingSo.buildingSprite;
            supplyCountText.text = buildingSo.resourceCost.ToString();
        }
    }
}
