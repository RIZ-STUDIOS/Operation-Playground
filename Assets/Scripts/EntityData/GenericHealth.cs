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

        private void Awake()
        {
            Health = MaxHealth;
        }

        public void Damage(int amount = 1)
        {
            if (amount < 0)
                return;

            Health -= amount;

            Debug.Log("Damage");

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
    }
}
