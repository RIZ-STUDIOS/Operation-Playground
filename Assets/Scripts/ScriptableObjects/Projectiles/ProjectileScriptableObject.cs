using RicTools;
using RicTools.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.ScriptableObjects.Projectiles
{
    public class ProjectileScriptableObject : GenericScriptableObject
    {
        public GameObject prefab;
        public float speed;
        public DamageType damageType;
        public ProjectileType projectileType;
    }

    public enum ProjectileType
    {
        Straight,
        Arc
    }

    public enum DamageType
    {
        None
    }
}