using OperationPlayground.ScriptableObjects;
using RicTools.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Buildings
{
    public class BuildingManager : MonoBehaviour
    {
        private AvailableBuildingsScriptableObject availableBuildings;

        private GameObject currentBuildingGameObject;
        private BuildingPlacement buildingPlacement;

        private void Awake()
        {
            availableBuildings = RicUtilities.GetAvailableScriptableObject<AvailableBuildingsScriptableObject, BuildingScriptableObject>();
        }

        private void StartPlacement(BuildingScriptableObject building)
        {
            currentBuildingGameObject = Instantiate(building.prefab, transform);

            currentBuildingGameObject.transform.localPosition = Vector3.forward * building.placementDistance;

            buildingPlacement = currentBuildingGameObject.AddComponent<BuildingPlacement>();
            buildingPlacement.toPlace = currentBuildingGameObject.GetComponent<Building>();
            buildingPlacement.toPlace.buildingSo = building;

            buildingPlacement.StartPlacement();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (buildingPlacement && Input.GetMouseButtonDown(0))
            {
                if (buildingPlacement.Place())
                {
                    buildingPlacement = null;
                    currentBuildingGameObject = null;
                }
            }

            if (!buildingPlacement && Input.GetMouseButtonDown(1))
            {
                StartPlacement(availableBuildings[0]);
            }
        }
#endif
    }
}
