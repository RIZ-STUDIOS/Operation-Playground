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
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField]
        private GameObject healthBarPrefab;

        [NonSerialized]
        public EnemyScriptableObject enemySo;

        private int health;

        public int Health { get { return health; } private set { health = value; healthSlider.value = health / (float)enemySo.health; } }

        private Slider healthSlider;

        private void Start()
        {
            var healthBar = Instantiate(healthBarPrefab, transform);
            healthBar.transform.localPosition = new Vector3(0, 2.5f, 0);

            healthSlider = healthBar.GetComponentInChildren<Slider>();
            var billboard = healthBar.GetComponentInChildren<Billboard>();
            billboard.lookAtTarget = GameManager.Instance.gameCamera.transform;

            Health = enemySo.health;
        }

        public void Damage(int amount = 1, DamageType damageType = DamageType.None)
        {
            if(amount < 0)
            {
                //Heal(amount, damageType);
                return;
            }

            int damageMod = IsWeakTo(damageType) ? 3 : 1;

            Health -= damageMod * amount;

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
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            EnemyRoundManager.Instance.RemoveEnemy(gameObject);
        }
    }
}
