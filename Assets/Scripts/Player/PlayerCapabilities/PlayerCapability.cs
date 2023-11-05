using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.PlayerCapabilities
{
    public enum PlayerCapabilityType
    {
        None,
        Camera,
        Graphics,
        Collision,
        Movement,
        Shooting
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
                case PlayerCapabilityType.Camera:
                    return new CameraPlayerCapability();
                case PlayerCapabilityType.Graphics:
                    return new GraphicsPlayerCapability();
                case PlayerCapabilityType.Collision:
                    return new CollisionPlayerCapability();
                case PlayerCapabilityType.Movement:
                    return new MovementPlayerCapability();
                case PlayerCapabilityType.Shooting:
                    return new ShootingPlayerCapability();
            }

            throw new System.Exception($"No set state for {stateType}");
        }
    }
}
