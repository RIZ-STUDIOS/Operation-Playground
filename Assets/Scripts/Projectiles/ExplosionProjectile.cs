using OperationPlayground.EntityData;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OperationPlayground.Projectiles
{
    [RequireComponent(typeof(StraightProjectile))]
    public class ExplosionProjectile : MonoBehaviour
    {
        [SerializeField]
        private float explosionRadius = 5;

        private StraightProjectile projectile;

        private void Awake()
        {
            projectile = GetComponent<StraightProjectile>();
            projectile.onCollision += OnCollision;
        }

        private void OnCollision(Collider hitCollider, Vector3 hitPoint)
        {
            var colliders = Physics.OverlapSphere(hitPoint, explosionRadius).ToList();
            if (colliders.Count == 0) return;

            foreach (var collider in colliders)
            {
                if(collider == hitCollider) continue;
                var genericEntity = collider.GetComponentInParent<GenericEntity>();
                if(!genericEntity) continue;

                if(projectile.shooter.parentEntity.Team == genericEntity.Team) continue;

                genericEntity.Health.Damage();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
