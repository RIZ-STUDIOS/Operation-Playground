using OperationPlayground.EntityData;
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

        private float gravityForce = 9.8f;

        private Vector3 startPosition;
        private Vector3 startPositionForward;

        private Vector3 currentPoint;
        private Vector3 previousPoint;

        [SerializeField]
        private ParticleSystem impactGround;

        [SerializeField]
        private ParticleSystem impactBlood;

        private bool hasCollided;

        private void Start()
        {
            startPosition = shooter.CurrentWeapon.FirePointTransform.position;
            startPositionForward = shooter.CurrentWeapon.FirePointTransform.forward.normalized;

            currentPoint = startPosition;
            previousPoint = startPosition;
        }

        protected override void Update()
        {
            base.Update();

            transform.position = FindPointOnParabola();
        }

        private void FixedUpdate()
        {
            if (!hasCollided)
            {
                timer += Time.fixedDeltaTime;

                MoveCurrentPoint();

                if (previousPoint != currentPoint)
                {
                    if (CastRayBetweenPoints(currentPoint, previousPoint, out RaycastHit prevHit))
                    {
                        HitQuery(prevHit);
                    }
                }

                if (CastRayBetweenPoints(currentPoint, previousPoint, out RaycastHit currHit))
                {
                    HitQuery(currHit);
                }

                previousPoint = currentPoint;
            }
        }

        public override void MoveCurrentPoint()
        {
            currentPoint = FindPointOnParabola();
        }

        private Vector3 FindPointOnParabola()
        {
            Vector3 point = startPosition + (startPositionForward * projectileSo.speed * timer);
            Vector3 gravityVector = Vector3.down * gravityForce * timer * timer;

            return point + gravityVector;
        }

        private bool CastRayBetweenPoints(Vector3 startPoint, Vector3 endPoint, out RaycastHit hit)
        {
            return Physics.Raycast(startPoint, endPoint - startPoint, out hit, (endPoint - startPoint).magnitude);
        }

        private void HitQuery(RaycastHit hit)
        {
            if (hit.collider.isTrigger) return;

            hasCollided = true;

            var entity = hit.collider.GetComponent<GenericEntity>();

            if (!entity)
            {
                //StartCoroutine(DestroyAfterFX(impactGround));
                Destroy();
                //return;
            }

            if (entity.Team == shooter.parentEntity.Team) Destroy();

            entity.Health.Damage();
            Destroy();
            //StartCoroutine(DestroyAfterFX(impactBlood));
        }

        private IEnumerator DestroyAfterFX(ParticleSystem fx)
        {
            fx.Play();

            while (fx.isPlaying) yield return null;

            Destroy();
        }

        protected override bool KeepAlive()
        {
            return timer < projectileSo.aliveTime;
        }
    }
}
