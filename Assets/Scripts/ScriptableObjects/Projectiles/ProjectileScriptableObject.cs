using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RicTools;
using RicTools.ScriptableObjects;

namespace OperationPlayground.ScriptableObjects.Projectiles
{
    public class ProjectileScriptableObject : GenericScriptableObject
    {
        public GameObject prefab;
        public float speed;
        public float aliveTime;
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