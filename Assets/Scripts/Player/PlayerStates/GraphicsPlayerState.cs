using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerStates
{
    public class GraphicsPlayerState : PlayerState
    {
        public override PlayerStateType StateType => PlayerStateType.Graphics;

        public override void OnStateEnter()
        {
            foreach (var renderer in playerManager.playerRenderers)
            {
                renderer.enabled = true;
            }
        }

        public override void OnStateLeave()
        {
            foreach (var renderer in playerManager.playerRenderers)
            {
                renderer.enabled = false;
            }
        }
    }
}
