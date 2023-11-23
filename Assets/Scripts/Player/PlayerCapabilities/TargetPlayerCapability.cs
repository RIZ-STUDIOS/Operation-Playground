using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public class TargetPlayerCapability : PlayerCapability
    {
        public override void OnStateEnter()
        {
            playerManager.targettable = true;
        }

        public override void OnStateLeave()
        {
            playerManager.targettable = false;
        }
    }
}
