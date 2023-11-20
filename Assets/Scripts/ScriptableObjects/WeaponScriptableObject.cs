using OperationPlayground.ScriptableObjects.Projectiles;
using RicTools;
using RicTools.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.ScriptableObjects
{
    public class WeaponScriptableObject : GenericScriptableObject
    {
        public GameObject prefab;
        public float cooldown;
        public int maxAmmo;
        public ProjectileScriptableObject projectileScriptableObject;
        public Vector3 slotOffset;
        public Sprite weaponSprite;
    }
}