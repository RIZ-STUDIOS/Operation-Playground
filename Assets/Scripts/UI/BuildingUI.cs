using OperationPlayground.Player;
using OperationPlayground.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OperationPlayground.UI
{
    public class BuildingUI : MonoBehaviour
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
