using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public class InvalidPlacementPlayerCapability : PlayerCapability
    {
        public override void OnStateEnter()
        {
            playerManager.InvaidPlacement.invalid = true;
        }

        public override void OnStateLeave()
        {
            playerManager.InvaidPlacement.invalid = false;
        }
    }
}
