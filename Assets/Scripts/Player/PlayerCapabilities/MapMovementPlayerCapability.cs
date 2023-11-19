using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public class MapMovementPlayerCapability : PlayerCapability
    {
        public override void OnStateEnter()
        {
            playerManager.PlayerMovement.enabled = true;
        }

        public override void OnStateLeave()
        {
            playerManager.PlayerMovement.enabled = false;
        }
    }
}
