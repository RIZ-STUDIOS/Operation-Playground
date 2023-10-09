using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerStates
{
    public class InvalidPlacementPlayerState : PlayerState
    {
        public override PlayerStateType StateType => PlayerStateType.InvalidPlacement;

        public override void OnStateEnter()
        {
            playerManager.invalidPlacement.invalid = true;
        }

        public override void OnStateLeave()
        {
            playerManager.invalidPlacement.invalid = false;
        }
    }
}
