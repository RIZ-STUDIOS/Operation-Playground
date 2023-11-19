using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public class TPSLookPlayerCapability : PlayerCapability
    {
        public override void OnStateEnter()
        {
            playerManager.PlayerMovementTPS.enabled = true;
        }

        public override void OnStateLeave()
        {
            playerManager.PlayerMovementTPS.enabled = false;
        }
    }
}
