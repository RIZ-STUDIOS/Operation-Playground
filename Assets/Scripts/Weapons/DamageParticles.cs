using RicTools.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public class DamageParticles : MonoBehaviour
    {
        [MinValue(0.001f)]
        public float damagePerSecond;

        private ParticleSystem particles;

        [System.NonSerialized]
        public ObjectHealth shooter;

        private void Awake()
        {
            particles = GetComponent<ParticleSystem>();
        }

        private void OnParticleCollision(GameObject other)
        {
            var collisionEvents = new List<ParticleCollisionEvent>();

            var numCollisionEvents = particles.GetCollisionEvents(other, collisionEvents);

            var objectHealth = other.GetComponentInParent<ObjectHealth>();

            if (!objectHealth) return;

            var canDamage = shooter ? shooter.IsPlayer != objectHealth.IsPlayer : true;

            if(!canDamage) return;

            var damagePerParticle = damagePerSecond / particles.emission.rateOverTime.constant;

            var damageAmount = numCollisionEvents * damagePerParticle;

            objectHealth.Damage(damageAmount);
        }
    }
}
