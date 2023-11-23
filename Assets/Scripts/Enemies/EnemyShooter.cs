using OperationPlayground.EntityData;
using OperationPlayground.Pathfinding;
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
        private float randomOffsetChangeTimer;

        private Vector3 targetOffset;

        private bool shooting;

        private FollowWaypoints followWaypoints;

        private void Awake()
        {
            followWaypoints = GetComponent<FollowWaypoints>();
            CalculateTargetOffset();
        }

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
            var position = followWaypoints.GetCurrentWaypointPosition();
            position.y = transform.position.y;
            transform.LookAt(position, Vector3.up);
        }

        private void AimAtTarget()
        {
            if (target)
            {
                randomOffsetChangeTimer += Time.deltaTime;
                if (randomOffsetChangeTimer >= 1.5f)
                {
                    CalculateTargetOffset();
                    randomOffsetChangeTimer = 0;
                }
                var targetPosition = target.transform.position;
                targetPosition.y = transform.position.y;
                transform.LookAt(targetPosition + targetOffset, Vector3.up);
            }
            else
            {
                ResetAim();
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!parentEntity.enemyScriptableObject) return;
            Gizmos.DrawWireSphere(transform.position, parentEntity.enemyScriptableObject.attackRange);
        }

        private void CalculateTargetOffset()
        {
            targetOffset = Random.insideUnitSphere * 0.5f;
        }
    }
}
