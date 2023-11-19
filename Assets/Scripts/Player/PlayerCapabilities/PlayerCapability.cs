using System;
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
        MovementInput,
        Shooter,
        Health,
        Interaction,
        Building,
        ToggleBuilding,
        InvalidPlacement,
        MapView,
        MapMovement,
        TPSMovement
    }

    public abstract class PlayerCapability
    {
        private static Dictionary<PlayerCapabilityType, System.Type> capabilitiesDictionary = new Dictionary<PlayerCapabilityType, System.Type>()
        {
            {PlayerCapabilityType.Camera, typeof(CameraPlayerCapability) },
            {PlayerCapabilityType.Graphics, typeof(GraphicsPlayerCapability) },
            {PlayerCapabilityType.Collision, typeof(CollisionPlayerCapability) },
            {PlayerCapabilityType.MovementInput, typeof(MovementInputPlayerCapability) },
            {PlayerCapabilityType.Shooter, typeof(ShootingPlayerCapability) },
            {PlayerCapabilityType.Health, typeof(HealthPlayerCapability) },
            {PlayerCapabilityType.Interaction, typeof(InteractablePlayerCapability) },
            {PlayerCapabilityType.Building, typeof(BuildingPlayerCapability) },
            {PlayerCapabilityType.ToggleBuilding, typeof(ToggleBuildingPlayerCapability) },
            {PlayerCapabilityType.InvalidPlacement, typeof(InvalidPlacementPlayerCapability) },
            {PlayerCapabilityType.MapView, typeof(MapViewPlayerCapability) },
            {PlayerCapabilityType.MapMovement, typeof(MapMovementPlayerCapability) },
            {PlayerCapabilityType.TPSMovement, typeof(MapViewPlayerCapability) },
        };

        public PlayerCapabilityType CapabilityType { get; private set; }

        public PlayerManager playerManager;

        public virtual void OnStateEnter() { }

        public virtual void OnStateLeave() { }

        public static PlayerCapability CreatePlayerCapability(PlayerCapabilityType stateType)
        {
            if (!capabilitiesDictionary.TryGetValue(stateType, out var capabilitiesType))
                throw new System.Exception($"No set state for {stateType}");

            var capability = (PlayerCapability)Activator.CreateInstance(capabilitiesType);
            capability.CapabilityType = stateType;
            return capability;
        }
    }
}
