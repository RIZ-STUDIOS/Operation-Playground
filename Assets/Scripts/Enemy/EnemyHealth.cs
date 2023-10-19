using OperationPlayground.Managers;
using OperationPlayground.ScriptableObjects;
using OperationPlayground.UI;
using RicTools;
using RicTools.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace OperationPlayground.Enemy
{
    public class EnemyHealth : ObjectHealth
    {
        [NonSerialized]
        public EnemyScriptableObject enemySo;

        public override float MaxHealth => enemySo.health;

        protected override void Start()
        {
            base.Start();
        }

        public new void Damage(float amount = 1)
        {
            Damage(amount, DamageType.None);
        }

        public void Damage(float amount = 1, DamageType damageType = DamageType.None)
        {
            int damageMod = IsWeakTo(damageType) ? 3 : 1;

            Health -= damageMod * amount;

            base.Damage(damageMod * amount);
        }

        public bool IsWeakTo(DamageType damageType)
        {
            if (damageType == DamageType.None) return false;

            return enemySo.damageTypes.Contains(damageType);
        }

        protected override void OnDeath()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            GameManager.Instance.enemyRoundManager.RemoveEnemy(gameObject);
        }
    }
}
