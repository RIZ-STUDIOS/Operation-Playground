using OperationPlayground.Buildings;
using OperationPlayground.Projectiles;
using OperationPlayground.ScriptableObjects;
using RicTools;
using RicTools.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OperationPlayground.Enemy
{
    public class EnemyAttack : MonoBehaviour
    {
        [NonSerialized]
        public EnemyScriptableObject enemySo;
        private GameTimer timer;

        private void Start()
        {
            timer = TimerManager.Instance.CreateTimer(enemySo.attackCooldown, true);

            timer.Start();

            timer.onTick += Attack;
        }

        private void OnDrawGizmosSelected()
        {
            if (enemySo == null) return;

            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(transform.position, enemySo.attackRange);
        }

        private void Attack()
        {
            var colliders = Physics.OverlapSphere(transform.position, enemySo.attackRange).ToList();
            if (colliders.Count == 0) return;

            var enemyTargets = colliders.Select(e => e.GetComponentInParent<EnemyTarget>()).ToList().FindAll(e => e != null && e.visible);
            if (enemyTargets.Count == 0) return;

            var targets = enemySo.targetBuildings.ToList();

            foreach (var enemyTarget in enemyTargets)
            {
                var building = enemyTarget.GetComponent<BuildingHealth>();
                if (building)
                {
                    if (targets.Contains(building.buildingSo))
                    {
                        var objectHealth = enemyTarget.GetComponentInChildren<ObjectHealth>();
                        if (objectHealth != null)
                        {
                            AttackHealth(objectHealth);
                            return;
                        }
                    }
                }
            }

            foreach (var enemyTarget in enemyTargets)
            {
                if (enemyTarget.tag == "Player")
                {
                    var objectHealth = enemyTarget.GetComponentInChildren<ObjectHealth>();
                    if (objectHealth != null)
                    {
                        AttackHealth(objectHealth);
                        return;
                    }
                }
            }
        }

        private void AttackHealth(ObjectHealth objectHealth)
        {
            var gameObject = Instantiate(enemySo.projectileSo.prefab);
            gameObject.transform.position = transform.position + new Vector3(0, 1, 0);

            gameObject.transform.LookAt(objectHealth.transform);

            var projectile = gameObject.AddComponent<Projectile>();
            projectile.parentShooter = GetComponent<EnemyHealth>();
            projectile.projectileSo = enemySo.projectileSo;

            //objectHealth.Damage(1);
        }

        private void OnDestroy()
        {
            timer.Remove();
        }
    }
}