using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public class MapPlayerCapability : PlayerCapability
    {
        public override void OnStateEnter()
        {
            playerManager.playerInput.Basic.ZoomMap.Enable();
        }

        public override void OnStateLeave()
        {
            playerManager.playerInput.Basic.ZoomMap.Disable();
        }
    }
}
