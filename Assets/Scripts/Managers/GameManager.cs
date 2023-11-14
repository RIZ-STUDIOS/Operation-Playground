using OperationPlayground.ScriptableObjects;
using OperationPlayground.SupplyDrop;
using RicTools.Managers;
using RicTools.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Managers
{
    public class GameManager : SingletonGenericManager<GameManager>
    {
        public GameLevelData gameLevelData;
        public AvailableBuildingsScriptableObject availableBuildingsScriptableObject;
        public SupplyDropManager supplyDropManager;

        protected override void OnCreation()
        {
            availableBuildingsScriptableObject = RicUtilities.GetAvailableScriptableObject<AvailableBuildingsScriptableObject, BuildingScriptableObject>();
        }
    }
}
