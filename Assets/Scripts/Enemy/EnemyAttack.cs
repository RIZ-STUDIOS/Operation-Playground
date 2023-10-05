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

        GameTimer timer;

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
            if(colliders.Count == 0) return;

            var enemyTargets = colliders.FindAll(e => e.GetComponentInParent<EnemyTarget>() != null).Cast<EnemyTarget>().ToList();
            if (enemyTargets.Count == 0) return;
        }
    }
}
