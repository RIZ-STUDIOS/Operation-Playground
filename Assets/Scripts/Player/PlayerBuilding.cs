using OperationPlayground.Buildings;
using OperationPlayground.Player;
using OperationPlayground.ScriptableObjects;
using RicTools.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player
{
    public class PlayerBuilding : MonoBehaviour
    {
        private AvailableBuildingsScriptableObject availableBuildings;

        private GameObject currentBuildingGameObject;
        private BuildingPlacement buildingPlacement;

        private int currentIndex;

        private PlayerManager playerInputManager;

        private bool buildingMode;

        private bool buildingToggleEnabled;

        private void Awake()
        {
            availableBuildings = RicUtilities.GetAvailableScriptableObject<AvailableBuildingsScriptableObject, BuildingScriptableObject>();
            playerInputManager = GetComponent<PlayerManager>();

        }

        private void OnDisable()
        {
            DisableBuildingToggle();
        }

        private void OnEnable()
        {
            if (buildingToggleEnabled)
                EnableBuildingToggle();
        }

        public void EnableBuildingToggle()
        {
            playerInputManager.playerInput.Player.ToggleBuild.performed += OnToggleBuildPerformed;
            buildingToggleEnabled = true;
        }

        public void DisableBuildingToggle()
        {
            playerInputManager.playerInput.Player.ToggleBuild.performed -= OnToggleBuildPerformed;
            DisableBuildMode();
            buildingToggleEnabled = false;
        }

        private void EnablePlacing()
        {
            playerInputManager.playerInput.Player.Fire.performed += OnFirePerformed;
            playerInputManager.playerInput.Player.Cycle.performed += OnCyclePerformed;
        }

        private void DisablePlacing()
        {
            playerInputManager.playerInput.Player.Fire.performed -= OnFirePerformed;
            playerInputManager.playerInput.Player.Cycle.performed -= OnCyclePerformed;
        }

        private void OnToggleBuildPerformed(InputAction.CallbackContext value)
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

        private void DisableBuildMode()
        {
            if (buildingMode)
            {
                buildingPlacement = null;
                Destroy(currentBuildingGameObject);
                DisablePlacing();
            }
            buildingMode = false;
        }

        private void EnableBuildMode()
        {
            EnablePlacing();

            currentIndex = 0;

            StartPlacement(availableBuildings[currentIndex]);
            buildingMode = true;
        }

        private void ToggleBuildingMode()
        {
            if (buildingMode)
            {
                DisableBuildMode();
                playerInputManager.AddPlayerState(PlayerStates.PlayerStateType.Shooting);
            }
            else
            {
                playerInputManager.RemovePlayerState(PlayerStates.PlayerStateType.Shooting);
                EnableBuildMode();
            }
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
            buildingPlacement.playerPlacing = playerInputManager;
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
