using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerStates
{
    public class HealthBarPlayerState : PlayerState
    {
        public override PlayerStateType StateType => PlayerStateType.HealthBar;

        public override void OnStateEnter()
        {
            playerManager.playerHealth.ShowHealthBar();
        }

        public override void OnStateLeave()
        {
            playerManager.playerHealth.HideHealthBar();
        }
    }
}
