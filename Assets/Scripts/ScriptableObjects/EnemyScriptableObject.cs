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
        public GameObject prefab;
        public int maxHealth;
        public float speed;
        public WeaponScriptableObject weaponScriptableObject;
        public Sprite enemySprite;
        public float attackRange;
        public float targetCheckTime;
        public float attackDelayTime;
        public float shootingTime;
    }
}