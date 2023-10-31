using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public class GraphicsPlayerCapability : PlayerCapability
    {
        public override PlayerCapabilityType CapabilityType => PlayerCapabilityType.Graphics;

        public override void OnStateEnter()
        {
            foreach (var renderer in playerManager.PlayerRenderers)
            {
                renderer.enabled = true;
            }
        }

        public override void OnStateLeave()
        {
            foreach (var renderer in playerManager.PlayerRenderers)
            {
                renderer.enabled = false;
            }
        }
    }
}
