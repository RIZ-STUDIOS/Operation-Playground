using OperationPlayground.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public class EnemyAttack : MonoBehaviour
    {
        [NonSerialized]
        public EnemyScriptableObject enemySo;

        private void OnDrawGizmos()
        {
            if (enemySo == null) return;

            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(transform.position, enemySo.attackRange);
        }
    }
}
