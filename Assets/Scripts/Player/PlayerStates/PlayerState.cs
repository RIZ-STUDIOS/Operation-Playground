using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerStates
{
    public enum PlayerStateType
    {
        None,
        Movement,
        Building,
        Shooting,
        EnemyTarget,
        InvalidPlacement,
        HealthBar,
        Looking,
        Interaction,
        Graphics,
        Collision
    }

    public abstract class PlayerState
    {
        public abstract PlayerStateType StateType { get; }

        public PlayerManager playerManager;

        public virtual void OnStateEnter() { }

        public virtual void OnStateLeave() { }

        public static PlayerState CreatePlayerState(PlayerStateType stateType)
        {
            switch (stateType)
            {
                case PlayerStateType.Movement:
                    return new MovementPlayerState();
                case PlayerStateType.Building:
                    return new BuildingPlayerState();
                case PlayerStateType.Shooting:
                    return new ShootingPlayerState();
                case PlayerStateType.EnemyTarget:
                    return new EnemyTargetPlayerState();
                case PlayerStateType.InvalidPlacement:
                    return new InvalidPlacementPlayerState();
                case PlayerStateType.HealthBar:
                    return new HealthBarPlayerState();
                case PlayerStateType.Looking:
                    return new LookingPlayerState();
                case PlayerStateType.Interaction:
                    return new InteractionPlayerState();
                case PlayerStateType.Graphics:
                    return new GraphicsPlayerState();
                case PlayerStateType.Collision:
                    return new CollisionPlayerState();
            }

            throw new System.Exception($"No set state for {stateType}");
        }
    }
}
