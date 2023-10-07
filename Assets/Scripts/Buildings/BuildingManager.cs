using OperationPlayground.Player;
using OperationPlayground.ScriptableObjects;
using RicTools.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Buildings
{
    public class BuildingManager : MonoBehaviour
    {
        private AvailableBuildingsScriptableObject availableBuildings;

        private GameObject currentBuildingGameObject;
        private BuildingPlacement buildingPlacement;

        private int currentIndex;

        private PlayerInputManager playerInputManager;

        private PlayerShooting playerShooting;

        private bool buildingMode;

        private void Awake()
        {
            availableBuildings = RicUtilities.GetAvailableScriptableObject<AvailableBuildingsScriptableObject, BuildingScriptableObject>();
            playerInputManager = GetComponent<PlayerInputManager>();
            playerShooting = GetComponent<PlayerShooting>();

        }

        private void OnDisable()
        {
            playerInputManager.playerInput.Player.Build.performed -= OnBuildPerformed;
            DisableInput();

            if (currentBuildingGameObject != null)
            {
                currentBuildingGameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            playerInputManager.playerInput.Player.Build.performed += OnBuildPerformed;
            if (buildingMode)
            {
                EnableInput();

                if (currentBuildingGameObject != null)
                {
                    currentBuildingGameObject.SetActive(true);
                }
            }
        }

        private void EnableInput()
        {
            playerInputManager.playerInput.Player.Fire.performed += OnFirePerformed;
            playerInputManager.playerInput.Player.Cycle.performed += OnCyclePerformed;
        }

        private void DisableInput()
        {
            playerInputManager.playerInput.Player.Fire.performed -= OnFirePerformed;
            playerInputManager.playerInput.Player.Cycle.performed -= OnCyclePerformed;
        }

        private void OnBuildPerformed(InputAction.CallbackContext value)
        {
            ToggleBuildingMode();
        }

        private void OnFirePerformed(InputAction.CallbackContext value)
        {
            PlaceBuilding();
        }

        private void OnCyclePerformed(InputAction.CallbackContext value)
        {
            var data = value.ReadValue<float>();
            if (CycleIndex(data > 0))
            {
                Destroy(currentBuildingGameObject);

                StartPlacement(availableBuildings[currentIndex]);
            }
        }

        private void ToggleBuildingMode()
        {
            if (buildingMode)
            {
                Destroy(currentBuildingGameObject);
                playerShooting.EnableInput();
                DisableInput();
            }
            else
            {
                playerShooting.DisableInput();
                EnableInput();

                currentIndex = 0;

                StartPlacement(availableBuildings[currentIndex]);
            }

            buildingMode = !buildingMode;
        }

        private bool CycleIndex(bool up)
        {
            var previousIndex = currentIndex;
            currentIndex += up ? 1 : -1;
            if (currentIndex < 0)
                currentIndex = availableBuildings.items.Length - 1;
            if (currentIndex >= availableBuildings.items.Length)
                currentIndex = 0;
            return currentIndex != previousIndex;
        }

        private void StartPlacement(BuildingScriptableObject building)
        {
            currentBuildingGameObject = Instantiate(building.prefab, transform);

            currentBuildingGameObject.transform.localPosition = Vector3.forward * building.placementDistance;

            buildingPlacement = currentBuildingGameObject.AddComponent<BuildingPlacement>();
            buildingPlacement.toPlace = currentBuildingGameObject.GetComponent<BuildingHealth>();
            buildingPlacement.toPlace.buildingSo = building;

            buildingPlacement.StartPlacement();
        }

        private void PlaceBuilding()
        {
            if (!buildingPlacement) return;
            if (buildingPlacement.Place())
            {
                buildingPlacement = null;
                currentBuildingGameObject = null;

                StartPlacement(availableBuildings[currentIndex]);
            }
        }
    }
}
