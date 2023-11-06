using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public class CollisionPlayerCapability : PlayerCapability
    {
        public override void OnStateEnter()
        {
            foreach (var collider in playerManager.PlayerColliders)
            {
                collider.enabled = true;
            }
        }

        public override void OnStateLeave()
        {
            foreach (var collider in playerManager.PlayerColliders)
            {
                collider.enabled = false;
            }
        }
    }
}
