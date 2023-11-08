using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public class HealthPlayerCapability : PlayerCapability
    {
        public override void OnStateEnter()
        {
            playerManager.PlayerHealth.VisibleHealthBar = true;
        }

        public override void OnStateLeave()
        {
            playerManager.PlayerHealth.VisibleHealthBar = false;
        }
    }
}
