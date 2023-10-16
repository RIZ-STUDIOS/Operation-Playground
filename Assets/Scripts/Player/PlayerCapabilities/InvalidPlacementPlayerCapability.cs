using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public class InvalidPlacementPlayerCapability : PlayerCapability
    {
        public override PlayerCapabilityType CapabilityType => PlayerCapabilityType.InvalidPlacement;

        public override void OnStateEnter()
        {
            playerManager.invalidPlacement.invalid = true;
        }

        public override void OnStateLeave()
        {
            playerManager.invalidPlacement.invalid = false;
        }
    }
}
