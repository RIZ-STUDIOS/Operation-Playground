using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RicTools;
using RicTools.ScriptableObjects;

namespace OperationPlayground.ScriptableObjects
{
    public class EnemyScriptableObject : GenericScriptableObject
    {
        public GameObject prefab;
        public int health;
        public DamageType[] damageTypes;
    }

    public enum DamageType
    {
        None,
        Explosion
    }
}