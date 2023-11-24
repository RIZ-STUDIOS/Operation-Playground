using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public class MapLookPlayerCapability : PlayerCapability
    {
        public override void OnStateEnter()
        {
            playerManager.PlayerLookMap.enabled = true;
        }

        public override void OnStateLeave()
        {
            playerManager.PlayerLookMap.enabled = false;
            playerManager.PlayerMap.ResetZoom();
        }
    }
}
