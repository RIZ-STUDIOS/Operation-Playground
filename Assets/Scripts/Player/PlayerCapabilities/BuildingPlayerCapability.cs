using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public class BuildingPlayerCapability : PlayerCapability
    {
        public override PlayerCapabilityType CapabilityType => PlayerCapabilityType.Building;

        public override void OnStateEnter()
        {
            playerManager.playerBuilding.EnableBuildingToggle();
        }

        public override void OnStateLeave()
        {
            playerManager.playerBuilding.DisableBuildingToggle();
        }
    }
}
