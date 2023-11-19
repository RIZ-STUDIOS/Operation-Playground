using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public class MovementInputPlayerCapability : PlayerCapability
    {
        public override void OnStateEnter()
        {
            playerManager.playerInput.Movement.Enable();
        }

        public override void OnStateLeave()
        {
            playerManager.playerInput.Movement.Disable();
        }
    }
}
