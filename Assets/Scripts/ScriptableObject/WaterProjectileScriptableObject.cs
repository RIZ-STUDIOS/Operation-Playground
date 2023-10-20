using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.ScriptableObjects
{
    public class WaterProjectileScriptableObject : ProjectileScriptableObject
    {
        public float beamDuration;
        public float beamCooldown;
        public float damagePerSecond;
        public float beamStrength;

        public bool IsContinuous => beamDuration <= 0;
        public bool IsSingleFire => beamCooldown < 0;
    }
}
