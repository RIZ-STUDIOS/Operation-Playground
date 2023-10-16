using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public class EnemyTargetPlayerCapability : PlayerCapability
    {
        public override PlayerCapabilityType CapabilityType => PlayerCapabilityType.EnemyTarget;

        public override void OnStateEnter()
        {
            playerManager.enemyTarget.visible = true;
        }

        public override void OnStateLeave()
        {
            playerManager.enemyTarget.visible = false;
        }
    }
}
