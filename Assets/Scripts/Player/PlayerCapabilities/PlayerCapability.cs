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
        Movement,
        Shooter,
        Health,
        Interaction,
        Building,
        ToggleBuilding,
        InvalidPlacement,
        MapViewInput,
        MapLook,
        TPSLook,
        GunVisibility,
        Target,
    }

    public abstract class PlayerCapability
    {
        private static Dictionary<PlayerCapabilityType, System.Type> capabilitiesDictionary = new Dictionary<PlayerCapabilityType, System.Type>()
        {
            {PlayerCapabilityType.Camera, typeof(CameraPlayerCapability) },
            {PlayerCapabilityType.Graphics, typeof(GraphicsPlayerCapability) },
            {PlayerCapabilityType.Collision, typeof(CollisionPlayerCapability) },
            {PlayerCapabilityType.Movement, typeof(MovementPlayerCapability) },
            {PlayerCapabilityType.Shooter, typeof(ShootingPlayerCapability) },
            {PlayerCapabilityType.Health, typeof(HealthPlayerCapability) },
            {PlayerCapabilityType.Interaction, typeof(InteractablePlayerCapability) },
            {PlayerCapabilityType.Building, typeof(BuildingPlayerCapability) },
            {PlayerCapabilityType.ToggleBuilding, typeof(ToggleBuildingPlayerCapability) },
            {PlayerCapabilityType.InvalidPlacement, typeof(InvalidPlacementPlayerCapability) },
            {PlayerCapabilityType.MapViewInput, typeof(MapViewPlayerCapability) },
            {PlayerCapabilityType.MapLook, typeof(MapLookPlayerCapability) },
            {PlayerCapabilityType.TPSLook, typeof(TPSLookPlayerCapability) },
            {PlayerCapabilityType.GunVisibility, typeof(GunVisbilityPlayerCapability) },
            {PlayerCapabilityType.Target, typeof(TargetPlayerCapability) },
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
