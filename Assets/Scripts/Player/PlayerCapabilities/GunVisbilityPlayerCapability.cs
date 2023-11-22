using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public class GunVisbilityPlayerCapability : PlayerCapability
    {
        public override void OnStateEnter()
        {
            playerManager.Shooter.ShowWeapon();
        }

        public override void OnStateLeave()
        {
            playerManager.Shooter.HideWeapon();
        }
    }
}
