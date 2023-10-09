using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerStates
{
    public class EnemyTargetPlayerState : PlayerState
    {
        public override PlayerStateType StateType => PlayerStateType.EnemyTarget;

        public override void OnStateEnter()
        {
            playerManager.enemyTarget.visible = true;
        }

        public override void OnStateLeave()
        {
            playerManager.enemyTarget.visible = false;
        }
    }
}
