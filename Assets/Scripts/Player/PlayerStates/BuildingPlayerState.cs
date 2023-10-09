using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerStates
{
    public class BuildingPlayerState : PlayerState
    {
        public override PlayerStateType StateType => PlayerStateType.Building;

        public override void OnStateEnter()
        {
            playerManager.playerBuilding.EnableBuildingToggle();
        }

        public override void OnStateLeave()
        {
            playerManager.playerBuilding.DisableBuildingToggle();
        }
    }
}
