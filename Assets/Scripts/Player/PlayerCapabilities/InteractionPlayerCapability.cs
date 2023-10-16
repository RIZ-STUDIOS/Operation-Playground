using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public class InteractionPlayerCapability : PlayerCapability
    {
        public override PlayerCapabilityType CapabilityType => PlayerCapabilityType.Interaction;

        public override void OnStateEnter()
        {
            playerManager.playerInteraction.EnableInteraction();
        }

        public override void OnStateLeave()
        {
            playerManager.playerInteraction.DisableInteraction();
        }
    }
}
