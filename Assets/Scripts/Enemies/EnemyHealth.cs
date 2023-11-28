using OperationPlayground.EntityData;
using OperationPlayground.Managers;
using OperationPlayground.Rounds;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Enemies
{
    public class EnemyHealth : GenericHealth
    {
        private EnemyEntity _enemyEntity;

        public new EnemyEntity parentEntity
        {
            get
            {
                if (!_enemyEntity) _enemyEntity = base.parentEntity as EnemyEntity;
                return _enemyEntity;
            }
        }

        public override int MaxHealth => parentEntity.enemyScriptableObject.maxHealth;

        [SerializeField]
        private Vector3 healthBarSpawnOffset = new Vector3(0, 2.5f, 0);

        protected override Vector3 HealthBarSpawnOffset => healthBarSpawnOffset;

        [System.NonSerialized]
        public bool spawnAmmo;

        protected override void Awake()
        {
            OnDeath += () =>
            {
                RoundManager.Instance.EnemyKilled(parentEntity);
                CreateAmmoPickup();
            };
        }

        private void Start()
        {
            CreateHealthBar();
            Health = MaxHealth;
        }

        private void CreateAmmoPickup()
        {
            if (!spawnAmmo) return;
            var gameObject = Instantiate(PrefabsManager.Instance.data.ammoPickupPrefab);
            gameObject.transform.position = transform.position + new Vector3(0, 0.5f, 0);
        }
    }
}
