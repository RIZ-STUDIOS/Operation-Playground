using OperationPlayground.Enemy;
using OperationPlayground.Managers;
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

        public void Damage(int amount)
        {
            if (amount < 0)
                return;

            Health -= amount;

            if (health < 0)
            {
                OnDeath();
            }
        }

        protected abstract void OnDeath();
    }
}
