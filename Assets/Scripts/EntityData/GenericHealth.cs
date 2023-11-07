using OperationPlayground.Managers;
using OperationPlayground.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.EntityData
{
    public abstract class GenericHealth : MonoBehaviour
    {
        [System.NonSerialized]
        public GenericEntity parentEntity;

        private int health;

        public int Health { get { return health; } protected set { health = value; onHealthChanged?.Invoke(); } }

        public abstract int MaxHealth { get; }

        public float HealthPer => Health / (float)MaxHealth;

        public event System.Action onHealthChanged;
        public event System.Action onDeath;

        protected virtual Vector3 HealthBarSpawnOffset => new Vector3(0, 2.5f, 0);

        protected virtual bool DestroyOnDeath => true;

        private ProgressBarUI healthBar;

        private bool visibleHealthBar = true;

        public bool VisibleHealthBar { get { return visibleHealthBar; } set { visibleHealthBar = value; UpdateHealthbarVisibility(); } }

        protected virtual void Awake()
        {
            CreateHealthBar();
            Health = MaxHealth;
        }

        public void Damage(int amount = 1)
        {
            if (amount < 0)
                return;

            Health -= amount;

            if (health <= 0)
            {
                onDeath?.Invoke();
                if (DestroyOnDeath)
                {
                    Destroy(gameObject);
                }
            }
        }

        public void Heal(int amount)
        {
            if (amount < 0)
                return;

            Health += amount;
        }

        protected void CreateHealthBar()
        {
            if (healthBar) return;
            var healthBarObject = Instantiate(PrefabsManager.Instance.data.progressBarUIPrefab, transform);
            healthBarObject.transform.localPosition = HealthBarSpawnOffset;
            healthBar = healthBarObject.GetComponent<ProgressBarUI>();
            onHealthChanged += () =>
            {
                healthBar.PercentFilled = HealthPer;
            };

            UpdateHealthbarVisibility();
        }

        private void UpdateHealthbarVisibility()
        {
            if (!healthBar) return;
            healthBar.gameObject.SetActive(visibleHealthBar);
        }
    }
}
