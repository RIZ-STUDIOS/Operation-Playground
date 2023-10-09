using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerStates
{
    public class ShootingPlayerState : PlayerState
    {
        public override PlayerStateType StateType => PlayerStateType.Shooting;

        public override void OnStateEnter()
        {
            playerManager.playerShooting.EnableShooting();
        }

        public override void OnStateLeave()
        {
            playerManager.playerShooting.DisableShooting();
        }
    }
}
