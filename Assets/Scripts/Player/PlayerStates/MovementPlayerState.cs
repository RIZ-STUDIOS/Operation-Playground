using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerStates
{
    public class MovementPlayerState : PlayerState
    {
        public override PlayerStateType StateType => PlayerStateType.Movement;

        public override void OnStateEnter()
        {
            playerManager.playerMovement.EnableMovement();
        }

        public override void OnStateLeave()
        {
            playerManager.playerMovement.DisableMovement();
        }
    }
}
