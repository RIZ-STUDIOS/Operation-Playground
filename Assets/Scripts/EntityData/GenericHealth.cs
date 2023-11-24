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

        public int Health
        {
            get { return health; }
            protected set
            {
                health = value;
                if (health > MaxHealth) health = MaxHealth;
                UpdateHealthbarVisibility();
                onHealthChanged?.Invoke();
            }
        }

        public abstract int MaxHealth { get; }

        public float HealthPer => Health / (float)MaxHealth;

        public event System.Action onHealthChanged;
        public event System.Action onDeath;

        private bool dead;

        protected virtual Vector3 HealthBarSpawnOffset => new Vector3(0, 2.5f, 0);

        protected virtual Vector3 HealthBarSize => new Vector3(1, 0.25f, 1);

        protected virtual bool DestroyOnDeath => true;

        protected ProgressBarUI healthBar;

        private bool visibleHealthBar = true;

        private bool forceVisibleHealthBar;

        public bool VisibleHealthBar { get { return visibleHealthBar; } set { visibleHealthBar = value; UpdateHealthbarVisibility(); } }

        public bool ForceVisibleHealthBar { get { return forceVisibleHealthBar; } set { forceVisibleHealthBar = value; UpdateHealthbarVisibility(); } }

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

            if (health <= 0 && !dead)
            {
                dead = true;
                onDeath?.Invoke();
                if (DestroyOnDeath)
                {
                    Destroy(gameObject);
                }
            }
        }

        [ContextMenu("Kill Entity")]
        public void Kill()
        {
            Damage(MaxHealth);
        }

        public void Heal(int amount)
        {
            if (amount < 0)
                return;

            Health += amount;

            if(Health > 0)
            {
                dead = false;
            }
        }

        public void FullyHeal()
        {
            Heal(MaxHealth);
        }

        protected void CreateHealthBar()
        {
            if (healthBar) return;
            var healthBarObject = Instantiate(PrefabsManager.Instance.data.progressBarUIPrefab, transform);
            healthBarObject.transform.localScale = HealthBarSize;
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
            healthBar.gameObject.SetActive(forceVisibleHealthBar || (visibleHealthBar && Health < MaxHealth));
        }

        private void OnDrawGizmos()
        {
            try
            {
                Gizmos.DrawCube(transform.position + HealthBarSpawnOffset, Vector3.one);
            }
            catch { }
        }
    }
}
