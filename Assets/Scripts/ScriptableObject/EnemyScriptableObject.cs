using RicTools;
using RicTools.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.ScriptableObjects
{
    public class EnemyScriptableObject : GenericScriptableObject
    {
        public GameObject prefab;
        public int health;
        public DamageType[] damageTypes;
        public float attackRange;
        public float attackCooldown;
        public BuildingScriptableObject[] targetBuildings;
    }

    public enum DamageType
    {
        None,
        Explosion
    }
}