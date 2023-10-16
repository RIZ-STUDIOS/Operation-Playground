using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public class MovementPlayerCapability : PlayerCapability
    {
        public override PlayerCapabilityType CapabilityType => PlayerCapabilityType.Movement;

        public override void OnStateEnter()
        {
            playerManager.playerMovement.EnableMovement();
        }

        public override void OnStateLeave()
        {
            playerManager.playerMovement.DisableMovement();
        }
    }
}
