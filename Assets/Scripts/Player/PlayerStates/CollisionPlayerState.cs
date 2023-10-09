using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerStates
{
    public class CollisionPlayerState : PlayerState
    {
        public override PlayerStateType StateType => PlayerStateType.Collision;

        public override void OnStateEnter()
        {
            foreach (var collider in playerManager.playerColliders)
            {
                collider.enabled = true;
            }
        }

        public override void OnStateLeave()
        {
            foreach (var collider in playerManager.playerColliders)
            {
                collider.enabled = false;
            }
        }
    }
}
