using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public class HealthBarPlayerCapability : PlayerCapability
    {
        public override PlayerCapabilityType CapabilityType => PlayerCapabilityType.HealthBar;

        public override void OnStateEnter()
        {
            playerManager.playerHealth.ShowHealthBar();
        }

        public override void OnStateLeave()
        {
            playerManager.playerHealth.HideHealthBar();
        }
    }
}
