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
        public int damage;
        public ProjectileType projectileType;
    }

    public enum ProjectileType
    {
        Straight,
        Arc
    }
}