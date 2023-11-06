using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public class CameraPlayerCapability : PlayerCapability
    {
        public override void OnStateEnter()
        {
            playerManager.PlayerCamera.enabled = true;
        }

        public override void OnStateLeave()
        {
            playerManager.PlayerCamera.enabled = false;
        }
    }
}
