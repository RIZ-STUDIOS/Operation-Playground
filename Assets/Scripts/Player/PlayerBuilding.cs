using OperationPlayground.Buildings;
using OperationPlayground.Managers;
using OperationPlayground.Resources;
using OperationPlayground.ScriptableObjects;
using OperationPlayground.UI;
using RicTools.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player
{
    public class PlayerBuilding : MonoBehaviour
    {
        [SerializeField, MinValue(0.001f)]
        private float buildTime;

        [SerializeField, ColorUsage(false)]
        private Color allowedPlacementColor;

        [SerializeField, ColorUsage(false)]
        private Color invalidPlacementColor;

        [SerializeField, ColorUsage(false)]
        private Color notEnoughSuppliesColor;

        private float timer;

        private PlayerManager playerManager;

        private GameObject currentBuildingObject;
        private Renderer[] currentBuildingRenderers;
        private BuildingScriptableObject currentBuilding;

        private int selectedIndex;

        private bool isBuilding;

        private bool triggerDown;

        private AvailableBuildingsScriptableObject availableBuildings => GameManager.Instance.availableBuildingsScriptableObject;

        private bool hasPlayerShootingCapability;

        private RadialBarUI timerUI;

        private bool cachedCanPlace;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();

            {
                var timerUIObject = Instantiate(PrefabsManager.Instance.data.radialBarUIPrefab);

                timerUI = timerUIObject.GetComponent<RadialBarUI>();

                timerUI.transform.parent = transform;

                timerUIObject.SetActive(false);

                UpdateTimerUI();
            }

            playerManager.playerInput.Basic.ToggleBuild.performed += OnToggleBuildingPerformed;

            playerManager.playerInput.Building.Cycle.performed += OnCyclePerformed;

            playerManager.playerInput.Building.Place.performed += OnPlacePerformed;
            playerManager.playerInput.Building.Place.canceled += OnPlaceCanceled;
        }

        private void OnToggleBuildingPerformed(InputAction.CallbackContext context)
        {
            isBuilding = !isBuilding;

            if (!isBuilding)
            {
                playerManager.RemovePlayerState(PlayerCapabilities.PlayerCapabilityType.Building);
            }
            else
            {
                hasPlayerShootingCapability = playerManager.RemovePlayerState(PlayerCapabilities.PlayerCapabilityType.Shooter);
                playerManager.AddPlayerState(PlayerCapabilities.PlayerCapabilityType.Building);
                SelectBuilding(0);
            }
        }

        private void OnCyclePerformed(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<float>();
            if (value == 0) return;

            var index = selectedIndex;

            index += value > 0 ? 1 : -1;

            if (index < 0)
                index = availableBuildings.Items.Count - 1;
            else if (index >= availableBuildings.Items.Count)
                index = 0;

            if (index != selectedIndex)
                SelectBuilding(index);
        }

        private void OnPlacePerformed(InputAction.CallbackContext context)
        {
            triggerDown = true;
            UpdateTimerUI();
        }

        private void OnPlaceCanceled(InputAction.CallbackContext context)
        {
            triggerDown = false;
            timer = 0;
            UpdateTimerUI();
        }

        public void DisableBuilding()
        {
            DestroyCurrentBuilding();
            isBuilding = false;
            if (hasPlayerShootingCapability)
            {
                playerManager.AddPlayerState(PlayerCapabilities.PlayerCapabilityType.Shooter);
            }
        }

        private void DestroyCurrentBuilding()
        {
            if (timerUI)
            {
                timerUI.transform.parent = transform;
                timerUI.gameObject.SetActive(false);
            }

            if (currentBuildingObject)
                Destroy(currentBuildingObject);
            currentBuilding = null;
            currentBuildingObject = null;
            currentBuildingRenderers = null;
        }

        private void SelectBuilding(int index)
        {
            DestroyCurrentBuilding();
            currentBuilding = availableBuildings.items[index];

            selectedIndex = index;

            currentBuildingObject = Instantiate(currentBuilding.visual, playerManager.playerTransform);
            currentBuildingObject.transform.localPosition = Vector3.forward * currentBuilding.placementDistance;
            timerUI.transform.SetParent(currentBuildingObject.transform, false);
            timerUI.transform.localPosition = currentBuilding.timerOffset;

            currentBuildingRenderers = currentBuildingObject.GetComponentsInChildren<Renderer>();

            foreach (var renderer in currentBuildingRenderers)
            {
                renderer.material = MaterialsManager.Instance.data.placementMaterial;
            }

            cachedCanPlace = CanPlace();

            UpdateVisualMaterial();
        }

        private void Update()
        {
            cachedCanPlace = CanPlace();
            UpdateVisualMaterial();
            if (triggerDown && cachedCanPlace)
            {
                timer += Time.deltaTime;

                UpdateTimerUI();

                if (timer >= buildTime)
                {
                    var buildingObject = Instantiate(currentBuilding.prefab);
                    buildingObject.transform.position = currentBuildingObject.transform.position;
                    buildingObject.transform.rotation = currentBuildingObject.transform.rotation;

                    buildingObject.GetComponent<BuildingData>().buildingScriptableObject = currentBuilding;

                    var invalidPlacement = buildingObject.GetOrAddComponent<InvalidPlacement>();
                    invalidPlacement.invalid = true;

                    triggerDown = false;
                    UpdateTimerUI();
                }
            }
            else if (triggerDown && !cachedCanPlace)
            {
                timer = 0;
                UpdateTimerUI();
            }
        }

        private void UpdateTimerUI()
        {
            timerUI.gameObject.SetActive(triggerDown);
            timerUI.PercentFilled = timer / buildTime;
        }

        private bool CanPlace()
        {
            if (!currentBuildingObject) return false;
            if (ResourceManager.Instance.Supplies < currentBuilding.resourceCost) return false;
            var colliders = Physics.OverlapBox(currentBuildingObject.transform.position, currentBuilding.boundsToCheck / 2f, currentBuildingObject.transform.rotation, Physics.AllLayers, QueryTriggerInteraction.Ignore);

            var invalidPlacements = colliders.Select(c => c.GetComponentInParent<InvalidPlacement>()).ToList().FindAll(c => c != null && c.invalid);

            var playerIndex = invalidPlacements.FindIndex(c => c.GetComponentInParent<PlayerManager>() == playerManager);

            if (playerIndex >= 0)
                invalidPlacements.RemoveAt(playerIndex);

            return invalidPlacements.Count == 0;
        }

        private void UpdateVisualMaterial()
        {
            if (currentBuildingRenderers == null) return;

            if (ResourceManager.Instance.Supplies < currentBuilding.resourceCost)
            {
                var color = notEnoughSuppliesColor;
                color.a = 0.5f;
                foreach (var renderer in currentBuildingRenderers)
                {
                    renderer.material.SetColor("_BaseColor", color);
                }
                return;
            }

            if (cachedCanPlace)
            {
                var color = allowedPlacementColor;
                color.a = 0.5f;
                foreach (var renderer in currentBuildingRenderers)
                {
                    renderer.material.SetColor("_BaseColor", color);
                }
            }
            else
            {
                var color = invalidPlacementColor;
                color.a = 0.5f;
                foreach (var renderer in currentBuildingRenderers)
                {
                    renderer.material.SetColor("_BaseColor", color);
                }
            }
        }
    }
}
