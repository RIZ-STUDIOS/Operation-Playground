using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerStates
{
    public class InteractionPlayerState : PlayerState
    {
        public override PlayerStateType StateType => PlayerStateType.Interaction;

        public override void OnStateEnter()
        {
            playerManager.playerInteraction.EnableInteraction();
        }

        public override void OnStateLeave()
        {
            playerManager.playerInteraction.DisableInteraction();
        }
    }
}
