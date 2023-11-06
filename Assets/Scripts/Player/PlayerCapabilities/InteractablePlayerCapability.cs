using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public class InteractablePlayerCapability : PlayerCapability
    {
        public override void OnStateEnter()
        {
            playerManager.playerInput.Interaction.Enable();
        }

        public override void OnStateLeave()
        {
            playerManager.playerInput.Interaction.Disable();
        }
    }
}
