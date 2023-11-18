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

        Vector3 startPosition;
        Vector3 startPositionForward;

        Vector3 currentPoint;
        Vector3 previousPoint;

        private void Start()
        {
            startPosition = shooter.CurrentWeapon.ShootTransform.position;
            startPositionForward = shooter.CurrentWeapon.ShootTransform.forward.normalized;

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
            var entity = hit.collider.GetComponent<GenericEntity>();

            if (DestroyOnCollision(hit.collider, entity))
            {
                //onCollision?.Invoke(hit.collider);
                Destroy();
            }

            if (!entity) return;
            if (entity.Team == shooter.parentEntity.Team) return;

            entity.Health.Damage();
        }

        protected override bool KeepAlive()
        {
            return timer < projectileSo.aliveTime;
        }
    }
}
