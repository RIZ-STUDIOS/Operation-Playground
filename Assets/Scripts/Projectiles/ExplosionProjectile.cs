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

        [SerializeField]
        private int explosionDamage = 10;

        private StraightProjectile projectile;

        private AudioSource explosionSound;

        private void Awake()
        {
            projectile = GetComponent<StraightProjectile>();
            projectile.onCollision += OnCollision;

            explosionSound = GetComponentInChildren<AudioSource>();
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

                var distance = Vector3.Distance(genericEntity.transform.position, hitPoint);

                var damageFalloff = Mathf.Min(1, distance / explosionRadius);

                genericEntity.Health.Damage((int)Mathf.Ceil(explosionDamage * damageFalloff));
            }

            explosionSound.Play();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
