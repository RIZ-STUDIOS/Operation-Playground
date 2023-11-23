using OperationPlayground.EntityData;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OperationPlayground.Enemies
{
    public class EnemyShooter : GenericShooter
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

        private List<GenericEntity> nearbyEntities = new List<GenericEntity>();

        private GenericEntity target;

        private float argetCheckTime => parentEntity.enemyScriptableObject.targetCheckTime;

        private float AttackTime => parentEntity.enemyScriptableObject.attackDelayTime;

        private float ShootTime => parentEntity.enemyScriptableObject.shootingTime;

        private float targetCheckTimer;
        private float attackTimer;
        private float shootTimer;

        private bool shooting;

        private void Update()
        {
            targetCheckTimer += Time.deltaTime;

            if (targetCheckTimer >= argetCheckTime)
            {
                targetCheckTimer = 0;
                GetNearbyEntities();
                CalculateTarget();
            }

            AimAtTarget();

            if (target)
            {
                if (!shooting)
                {
                    attackTimer += Time.deltaTime;

                    if (attackTimer >= AttackTime)
                    {
                        attackTimer = 0;
                        shooting = true;
                    }
                }

                AttackTarget();
            }
        }

        private void GetNearbyEntities()
        {
            nearbyEntities.Clear();

            var colliders = Physics.OverlapSphere(transform.position, parentEntity.enemyScriptableObject.attackRange, Physics.AllLayers, QueryTriggerInteraction.Ignore).ToList();
            if (colliders.Count == 0) return;

            var entities = colliders.Select(c => c.GetComponentInParent<GenericEntity>()).ToList().FindAll(e => e != null && e.Team != parentEntity.Team && e.targettable);
            if(entities.Count == 0) return;

            nearbyEntities = entities;
        }

        private void CalculateTarget()
        {
            nearbyEntities = nearbyEntities.OrderBy((entity) => Vector3.Distance(entity.transform.position, transform.position)).ToList();
            target = null;

            if(nearbyEntities.Count > 0)
            {
                target = nearbyEntities[0];
            }


            if(!target)
            {
                shooting = false;
                attackTimer = 0;
                ResetAim();
            }
        }

        private void AttackTarget()
        {
            if (!target) return;
            if (!shooting) return;

            shootTimer += Time.deltaTime;

            CurrentWeapon.Shoot();

            if (shootTimer >= ShootTime)
            {
                shootTimer = 0;
                shooting = false;
            }
        }

        private void ResetAim()
        {

        }

        private void AimAtTarget()
        {
            if (!target) return;
        }

        private void OnDrawGizmosSelected()
        {
            if (!parentEntity.enemyScriptableObject) return;
            Gizmos.DrawWireSphere(transform.position, parentEntity.enemyScriptableObject.attackRange);
        }
    }
}
