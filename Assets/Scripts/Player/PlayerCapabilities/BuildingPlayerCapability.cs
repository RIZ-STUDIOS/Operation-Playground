using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public class BuildingPlayerCapability : PlayerCapability
    {
        public override void OnStateEnter()
        {
            playerManager.playerInput.Building.Enable();
            playerManager.playerInput.Basic.ToggleBuild.Enable();
        }

        public override void OnStateLeave()
        {
            playerManager.playerInput.Building.Disable();
            playerManager.playerInput.Basic.ToggleBuild.Disable();
            playerManager.PlayerBuilding.DisableBuilding();
        }
    }
}
