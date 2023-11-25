using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public class TPSLookPlayerCapability : PlayerCapability
    {
        public override void OnStateEnter()
        {
            playerManager.PlayerMovementTPS.enabled = true;
            playerManager.PlayerCanvas.ReticleUI.gameObject.SetActive(true);
        }

        public override void OnStateLeave()
        {
            playerManager.PlayerMovementTPS.enabled = false;
            playerManager.PlayerCanvas.ReticleUI.gameObject.SetActive(false);
        }
    }
}
