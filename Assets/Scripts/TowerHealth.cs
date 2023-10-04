using OperationPlayground.Managers;
using RicTools.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public class TowerHealth : MonoBehaviour
    {
        [SerializeField, MinValue(1), ReadOnly(AvailableMode.Play)]
        private int maxHealth;

        private int health;

        public int Health { get { return health; } private set { health = value; onHealthChange?.Invoke(); } }

        public int MaxHealth => maxHealth;

        public event System.Action onHealthChange;

        private void Awake()
        {
            GameManager.Instance.towerHealth = this;
            Health = maxHealth;
        }

        public void Damage(int amount)
        {
            if (amount < 0)
                return;

            Health -= amount;

            if(health < 0)
            {

            }
        }
    }
}
