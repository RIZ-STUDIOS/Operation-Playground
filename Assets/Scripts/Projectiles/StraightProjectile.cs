using OperationPlayground.ScriptableObjects.Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Projectiles
{
    public class StraightProjectile : Projectile
    {
        private new StraightProjectileScriptableObject projectileSo => base.projectileSo as StraightProjectileScriptableObject;

        private float timer;

        protected override void Update()
        {
            base.Update();

            timer += Time.deltaTime;
        }

        public override void Move()
        {
            transform.position += transform.forward * projectileSo.speed * Time.deltaTime;
        }

        protected override bool KeepAlive()
        {
            return timer < projectileSo.aliveTime;
        }
    }
}
