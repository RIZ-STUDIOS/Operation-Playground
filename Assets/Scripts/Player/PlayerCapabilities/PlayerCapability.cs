using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public enum PlayerCapabilityType
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

    public abstract class PlayerCapability
    {
        public abstract PlayerCapabilityType CapabilityType { get; }

        public PlayerManager playerManager;

        public virtual void OnStateEnter() { }

        public virtual void OnStateLeave() { }

        public static PlayerCapability CreatePlayerCapability(PlayerCapabilityType stateType)
        {
            switch (stateType)
            {
                case PlayerCapabilityType.Movement:
                    return new MovementPlayerCapability();
                case PlayerCapabilityType.Building:
                    return new BuildingPlayerCapability();
                case PlayerCapabilityType.Shooting:
                    return new ShootingPlayerCapability();
                case PlayerCapabilityType.EnemyTarget:
                    return new EnemyTargetPlayerCapability();
                case PlayerCapabilityType.InvalidPlacement:
                    return new InvalidPlacementPlayerCapability();
                case PlayerCapabilityType.HealthBar:
                    return new HealthBarPlayerCapability();
                case PlayerCapabilityType.Looking:
                    return new LookingPlayerCapability();
                case PlayerCapabilityType.Interaction:
                    return new InteractionPlayerCapability();
                case PlayerCapabilityType.Graphics:
                    return new GraphicsPlayerCapability();
                case PlayerCapabilityType.Collision:
                    return new CollisionPlayerCapability();
            }

            throw new System.Exception($"No set state for {stateType}");
        }
    }
}
