using OperationPlayground.EntityData;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OperationPlayground.Enemies
{
    [RequireComponent(typeof(SphereCollider))]
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

        private SphereCollider sphereCollider;

        private List<GenericEntity> nearbyEntities = new List<GenericEntity>();

        private GenericEntity target;

        private void Awake()
        {
            sphereCollider = GetComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            sphereCollider.radius = parentEntity.enemyScriptableObject.attackRange;
        }

        private void OnTriggerEnter(Collider other)
        {
            var entity = other.GetComponentInParent<GenericEntity>();
            if (!entity) return;

            nearbyEntities.Add(entity);

            CalculateTarget();
        }

        private void OnTriggerExit(Collider other)
        {
            var entity = other.GetComponentInParent<GenericEntity>();
            if (!entity) return;

            nearbyEntities.Remove(entity);

            CalculateTarget();
        }

        private void CalculateTarget()
        {
            nearbyEntities = (List<GenericEntity>)nearbyEntities.OrderBy((entity) => Vector3.Distance(entity.transform.position, transform.position));
        }
    }
}
