using OperationPlayground.EntityData;
using OperationPlayground.Pathfinding;
using OperationPlayground.ScriptableObjects;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Enemies
{
    [RequireComponent(typeof(EnemyHealth))]
    [RequireComponent(typeof(EnemyShooter))]
    public class EnemyEntity : GenericEntity
    {
        public EnemyScriptableObject enemyScriptableObject;

        public EnemyHealth EnemyHealth => this.GetIfNull(ref _enemyHealth);
        public EnemyShooter EnemyShooter => this.GetIfNull(ref _enemyShooter);

        private EnemyHealth _enemyHealth;
        private EnemyShooter _enemyShooter;

        public override GenericHealth Health => EnemyHealth;

        public override GenericShooter Shooter => EnemyShooter;

        public static GameObject SpawnEnemy(EnemyScriptableObject enemyScriptableObject, Vector3 spawnLocation = default)
        {
            var enemyObject = Instantiate(enemyScriptableObject.prefab);

            var enemy = enemyObject.GetComponent<EnemyEntity>();

            if (!enemy)
                throw new System.Exception();

            enemy.enemyScriptableObject = enemyScriptableObject;

            enemy.Shooter.AddWeapon(enemyScriptableObject.weaponScriptableObject);

            var followPath = enemyObject.GetComponent<FollowWaypoints>();
            if (followPath)
            {
                followPath.SetPosition(spawnLocation);
                followPath.SetSpeed(enemyScriptableObject.speed);
                followPath.onEndPathReached += () =>
                {
                    enemy.Health.Damage(1000);
                };
            }
            else
                enemy.transform.position = spawnLocation;

            return enemyObject;
        }
    }
}
