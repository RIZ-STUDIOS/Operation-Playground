using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public class LookingPlayerCapability : PlayerCapability
    {
        public override PlayerCapabilityType CapabilityType => PlayerCapabilityType.Looking;

        public override void OnStateEnter()
        {
            playerManager.playerMovement.EnableLooking();
        }

        public override void OnStateLeave()
        {
            playerManager.playerMovement.DisableLooking();
        }
    }
}
