using OperationPlayground.Player;
using OperationPlayground.Player.DefendPoint;
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
        public PlayerRespawnManager playerRespawnManager;
        public DefendPointData defendPointData;

        protected override void OnCreation()
        {
            availableBuildingsScriptableObject = RicUtilities.GetAvailableScriptableObject<AvailableBuildingsScriptableObject, BuildingScriptableObject>();
        }
    }
}
