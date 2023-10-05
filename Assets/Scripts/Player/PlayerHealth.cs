using OperationPlayground.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public class PlayerHealth : ObjectHealth
    {
        [SerializeField]
        private int maxHealth;

        [SerializeField]
        private GameObject healthBarPrefab;

        public override int MaxHealth => maxHealth;

        protected override void Start()
        {
            base.Start();

            var healthBar = Instantiate(healthBarPrefab, transform);
            healthBar.transform.localPosition = new Vector3(0, 2.5f, 0);

            var enemyHealthUI = healthBar.GetComponentInChildren<ObjectHealthUI>();
            enemyHealthUI.parentHealth = this;
        }

        protected override void OnDeath()
        {

        }
    }
}
