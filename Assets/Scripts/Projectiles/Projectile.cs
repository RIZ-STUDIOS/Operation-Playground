using OperationPlayground.EntityData;
using OperationPlayground.ScriptableObjects;
using OperationPlayground.ScriptableObjects.Projectiles;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace OperationPlayground.Projectiles
{
    //[RequireComponent(typeof(Collider))]
    public abstract class Projectile : MonoBehaviour
    {
        protected ProjectileScriptableObject projectileSo;

        [System.NonSerialized]
        public GenericShooter shooter;

        public System.Action<Collider, Vector3> onCollision;

        public AudioSource projectileLaunchSound;

        protected Rigidbody rb;

        private bool hasSound;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public static GameObject CreateProjectile(ProjectileScriptableObject projectileScriptableObject, GenericShooter shooter)
        {
            var projectileObject = Instantiate(projectileScriptableObject.prefab);

            var projectile = projectileObject.GetComponent<Projectile>();
            if (!projectile) throw new System.Exception($"No Projectile component in projectile's prefab '{projectileScriptableObject.id}'");

            projectile.projectileSo = projectileScriptableObject;
            projectile.shooter = shooter;

            return projectileObject;
        }

        public abstract void MoveCurrentPoint();

        protected virtual void Update()
        {
            if (!KeepAlive())
            {
                Destroy();
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        /*private void OnTriggerEnter(Collider other)
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
        }*/

        protected virtual bool DestroyOnCollision(Collider other, GenericEntity hitEntity)
        {
            if (!hitEntity) return true;
            return hitEntity != shooter.parentEntity;
        }

        protected abstract bool KeepAlive();
    }
}
