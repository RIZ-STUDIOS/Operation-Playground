using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public class ToggleBuildingPlayerCapability : PlayerCapability
    {
        public override void OnStateEnter()
        {
            playerManager.playerInput.Basic.ToggleBuild.Enable();
        }

        public override void OnStateLeave()
        {
            playerManager.playerInput.Basic.ToggleBuild.Disable();
        }
    }
}
