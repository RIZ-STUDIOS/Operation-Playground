using OperationPlayground.ScriptableObjects;
using OperationPlayground.ScriptableObjects.Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Projectiles
{
    [RequireComponent(typeof(Collider))]
    public class Projectile : MonoBehaviour
    {
        private ProjectileScriptableObject projectileSo;

        [System.NonSerialized]
        public GenericShooter shooter;

        public static GameObject CreateProjectile(ProjectileScriptableObject projectileScriptableObject, GenericShooter shooter)
        {
            var projectileObject = Instantiate(projectileScriptableObject.prefab);

            var projectile = projectileObject.GetComponent<Projectile>();
            if (!projectile) throw new System.Exception($"No Projectile component in projectile's prefab '{projectileScriptableObject.id}'");

            projectile.projectileSo = projectileScriptableObject;
            projectile.shooter = shooter;

            var rb = projectileObject.GetComponent<Rigidbody>();
            if(!rb) projectileObject.AddComponent<Rigidbody>();

            rb.isKinematic = true;

            var collider = projectileObject.GetComponent<Collider>();
            collider.isTrigger = true;

            return projectileObject;
        }

        public virtual void Move()
        {

        }
    }
}
