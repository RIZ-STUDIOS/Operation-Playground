using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public class ShootingPlayerCapability : PlayerCapability
    {
        public override void OnStateEnter()
        {
            playerManager.playerInput.Shoot.Enable();
            playerManager.PlayerShooter.ShowWeapon();
        }

        public override void OnStateLeave()
        {
            playerManager.playerInput.Shoot.Disable();
            playerManager.PlayerShooter.HideWeapon();
        }
    }
}
