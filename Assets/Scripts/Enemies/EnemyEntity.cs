using OperationPlayground.EntityData;
using OperationPlayground.Managers;
using OperationPlayground.Pathfinding;
using OperationPlayground.ScriptableObjects;
using OperationPlayground.Weapons;
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

        private FollowWaypoints followWaypoints;

        private float timer;

        public static GameObject SpawnEnemy(EnemyScriptableObject enemyScriptableObject, Vector3 spawnLocation = default)
        {
            var enemyObject = Instantiate(enemyScriptableObject.prefab);

            var enemy = enemyObject.GetComponent<EnemyEntity>();

            if (!enemy)
                throw new System.Exception("NOT AN ENEMY");

            enemy.enemyScriptableObject = enemyScriptableObject;

            var weapon = enemy.GetComponentInChildren<Weapon>();
            if (!weapon)
            {
                enemy.Shooter.AddWeapon(enemyScriptableObject.weaponScriptableObject);
                enemy.Shooter.CurrentWeapon.InfiniteAmmo = true;
            }
            else
            {
                weapon.weaponSo = enemyScriptableObject.weaponScriptableObject;
                weapon.InfiniteAmmo = true;
                enemy.Shooter.SetWeapon(weapon);
            }

            var followPath = enemyObject.GetComponent<FollowWaypoints>();
            enemy.followWaypoints = followPath;
            if (followPath)
            {
                followPath.SetPosition(spawnLocation);
                followPath.SetSpeed(enemyScriptableObject.speed);
            }
            else
                enemy.transform.position = spawnLocation;

            return enemyObject;
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if(timer >= 5)
            {
                followWaypoints.CalculateNewPath();
                timer = 0;
            }
        }
    }
}
