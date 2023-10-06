using OperationPlayground.Enemy;
using OperationPlayground.Managers;
using OperationPlayground.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public abstract class ObjectHealth : MonoBehaviour
    {
        private int health;

        public int Health { get { return health; } protected set { health = value; onHealthChange?.Invoke(); } }

        public abstract int MaxHealth { get; }

        public event System.Action onHealthChange;

        public float HealthPer => Health / (float)MaxHealth;

        protected virtual bool DoSpawnHealthBar => true;

        protected virtual Vector3 HealthBarSpawnOffset => new Vector3(0, 2.5f, 0);

        private GameObject healthBarPrefab => PrefabsManager.Instance.data.healthBarPrefab;

        public virtual bool IsPlayer => false;

        protected virtual void Start()
        {
            Health = MaxHealth;

            if(DoSpawnHealthBar)
                SpawnHealthBar();
        }

        public void Damage(int amount)
        {
            if (amount < 0)
                return;

            Health -= amount;

            if (health <= 0)
            {
                OnDeath();
            }
        }

        protected abstract void OnDeath();

        protected void SpawnHealthBar()
        {
            var healthBar = Instantiate(healthBarPrefab, transform);
            healthBar.transform.localPosition = HealthBarSpawnOffset;

            var enemyHealthUI = healthBar.GetComponentInChildren<ObjectHealthUI>();
            enemyHealthUI.parentHealth = this;
        }
    }
}
