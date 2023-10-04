using RicTools.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OperationPlayground.Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        [ReadOnly(AvailableMode.Editor), MustBeAssigned]
        public EnemyScriptableObject enemySo;

        private int health;

        public int Health { get { return health; } private set { health = value; } }

        private void Start()
        {
            health = enemySo.health;
        }

        public void Damage(int amount = 1, DamageType damageType = DamageType.None)
        {
            if(amount < 0)
            {
                //Heal(amount, damageType);
                return;
            }

            int damageMod = IsWeakTo(damageType) ? 3 : 1;

            health -= damageMod * amount;

            if(health <= 0)
            {
                DestroyEnemy();
            }
        }

        public bool IsWeakTo(DamageType damageType)
        {
            if(damageType == DamageType.None) return false;

            return enemySo.damageTypes.Contains(damageType);
        }

        public void DestroyEnemy()
        {
            EnemyRoundManager.Instance.RemoveEnemy(gameObject);
            Destroy(gameObject);
        }
    }
}
