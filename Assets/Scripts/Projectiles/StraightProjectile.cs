using OperationPlayground.EntityData;
using OperationPlayground.ScriptableObjects.Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Projectiles
{
    public class StraightProjectile : Projectile
    {
        private const float FORCE_GRAVITY = 9.8f;

        private new StraightProjectileScriptableObject projectileSo => base.projectileSo as StraightProjectileScriptableObject;

        private float timer;

        private Vector3 startPosition;
        private Vector3 startPositionForward;

        private Vector3 currentPoint;
        private Vector3 previousPoint;

        [SerializeField]
        private ParticleSystem impactGround;

        [SerializeField]
        private ParticleSystem impactBlood;

        [SerializeField]
        private MeshRenderer bulletRender;

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
        }

        private void FixedUpdate()
        {
            if (!hasCollided)
            {
                timer += Time.fixedDeltaTime;

                MoveCurrentPoint();
                transform.position = currentPoint;

                if (previousPoint != currentPoint)
                {
                    if (CastRayBetweenTwoPoints(previousPoint, currentPoint, out RaycastHit previousHit))
                    {
                        HitQuery(previousHit);
                    }
                }

                if (CastRayBetweenTwoPoints(previousPoint, currentPoint, out RaycastHit currentHit))
                {
                    HitQuery(currentHit);
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
            Vector3 point = startPosition + (projectileSo.speed * timer * startPositionForward);
            Vector3 gravityVector = FORCE_GRAVITY * timer * timer * Vector3.down;

            return point + gravityVector;
        }

        private bool CastRayBetweenTwoPoints(Vector3 startPoint, Vector3 endPoint, out RaycastHit hit)
        {
            return Physics.Raycast(startPoint, endPoint - startPoint, out hit, (endPoint - startPoint).magnitude, Physics.AllLayers ^ LayerMask.GetMask("InvisibleInvalidPlacement", "EnemyPath"), QueryTriggerInteraction.Ignore);
        }

        private void HitQuery(RaycastHit hit)
        {
            hasCollided = true;
            transform.position = hit.point;

            var entity = hit.collider.GetComponent<GenericEntity>();

            if (!entity)
            {
                StartCoroutine(DestroyAfterFX(impactGround));
                return;
            }

            if (entity.Team == shooter.parentEntity.Team)
            {
                Destroy();
                return;
            }

            entity.Health.Damage();
            StartCoroutine(DestroyAfterFX(impactBlood));
        }

        private IEnumerator DestroyAfterFX(ParticleSystem fx)
        {
            bulletRender.enabled = false;

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
