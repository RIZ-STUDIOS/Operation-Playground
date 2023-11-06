using OperationPlayground.EntityData;
using OperationPlayground.ScriptableObjects;
using OperationPlayground.ScriptableObjects.Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Projectiles
{
    [RequireComponent(typeof(Collider))]
    public abstract class Projectile : MonoBehaviour
    {
        protected ProjectileScriptableObject projectileSo;

        [System.NonSerialized]
        public GenericShooter shooter;

        public event System.Action<Collider> onCollision;

        public static GameObject CreateProjectile(ProjectileScriptableObject projectileScriptableObject, GenericShooter shooter)
        {
            var projectileObject = Instantiate(projectileScriptableObject.prefab);

            var projectile = projectileObject.GetComponent<Projectile>();
            if (!projectile) throw new System.Exception($"No Projectile component in projectile's prefab '{projectileScriptableObject.id}'");

            projectile.projectileSo = projectileScriptableObject;
            projectile.shooter = shooter;

            var rb = projectileObject.GetComponent<Rigidbody>();
            if (!rb) projectileObject.AddComponent<Rigidbody>();

            rb.isKinematic = true;

            var collider = projectileObject.GetComponent<Collider>();
            collider.isTrigger = true;

            return projectileObject;
        }

        public abstract void Move();

        protected virtual void Update()
        {
            Move();

            if (!KeepAlive())
            {
                Destroy();
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger) return;
            var entity = other.GetComponent<GenericEntity>();

            if (DestroyOnCollision(other, entity))
            {
                onCollision?.Invoke(other);
                Destroy();
            }

            if (!entity) return;
            if (entity.Team == shooter.parentEntity.Team) return;

            entity.Health.Damage();
        }

        protected virtual bool DestroyOnCollision(Collider other, GenericEntity hitEntity)
        {
            if (!hitEntity) return true;
            return hitEntity != shooter.parentEntity;
        }

        protected abstract bool KeepAlive();
    }
}
