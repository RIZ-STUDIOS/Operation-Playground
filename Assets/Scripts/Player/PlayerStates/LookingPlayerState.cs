using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerStates
{
    public class LookingPlayerState : PlayerState
    {
        public override PlayerStateType StateType => PlayerStateType.Looking;

        public override void OnStateEnter()
        {
            playerManager.playerMovement.EnableLooking();
        }

        public override void OnStateLeave()
        {
            playerManager.playerMovement.DisableLooking();
        }
    }
}
