using OperationPlayground.ScriptableObjects.Projectiles;
using RicTools;
using RicTools.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.ScriptableObjects
{
    public class EnemyScriptableObject : GenericScriptableObject
    {
        public int maxHealth;
        public DamageType[] vulnerableDamageTypes;
        public WeaponScriptableObject weaponScriptableObject;
    }
}