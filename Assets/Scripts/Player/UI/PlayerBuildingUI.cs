using OperationPlayground.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OperationPlayground.Player.UI
{
    public class PlayerBuildingUI : MonoBehaviour
    {
        private PlayerManager playerManager;
        private CanvasGroup canvasGroup;

        [SerializeField]
        private Image buildingImage;

        [SerializeField]
        private TextMeshProUGUI supplyCountText;

        private void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            canvasGroup = GetComponent<CanvasGroup>();

            playerManager.PlayerBuilding.onEnterBuildingMode += () => canvasGroup.alpha = 1;
            playerManager.PlayerBuilding.onExitBuildingMode += () => canvasGroup.alpha = 0;

            playerManager.PlayerBuilding.onBuildingSelected += OnBuildingSelected;
        }

        private void OnBuildingSelected(BuildingScriptableObject buildingSo)
        {
            buildingImage.sprite = buildingSo.buildingSprite;
            supplyCountText.text = buildingSo.resourceCost.ToString();
        }
    }
}
