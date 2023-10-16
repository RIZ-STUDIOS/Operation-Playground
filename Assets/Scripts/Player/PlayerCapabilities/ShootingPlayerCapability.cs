using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public class ShootingPlayerCapability : PlayerCapability
    {
        public override PlayerCapabilityType CapabilityType => PlayerCapabilityType.Shooting;

        public override void OnStateEnter()
        {
            playerManager.playerShooting.EnableShooting();
        }

        public override void OnStateLeave()
        {
            playerManager.playerShooting.DisableShooting();
        }
    }
}
