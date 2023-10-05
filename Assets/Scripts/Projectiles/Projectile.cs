using OperationPlayground.ScriptableObjects;
using RicTools;
using RicTools.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        [System.NonSerialized]
        public ProjectileScriptableObject projectileSo;

        private float groundOffset;

        float timer;

        private void Start()
        {
            groundOffset = GetGroundOffset();
        }

        private void Update()
        {
            timer += Time.deltaTime;

            Move();

            if(timer >= projectileSo.travelDuration)
            {
                Destroy();
            }
        }

        private bool GetGroundHitInfo(out RaycastHit hitInfo)
        {
            return Physics.Raycast(transform.position, Vector3.down, out hitInfo, float.MaxValue, LayerMask.GetMask("Ground"));
        }

        private float GetGroundOffset()
        {
            if(!GetGroundHitInfo(out var hitInfo))
            {
                return groundOffset;
            }
            return hitInfo.distance;
        }

        private void Move()
        {
            transform.position += transform.forward * projectileSo.speed * Time.deltaTime;

            var currentGroundOffset = GetGroundOffset();

            if(currentGroundOffset != groundOffset)
            {
                var position = transform.position;
                GetGroundHitInfo(out var hitInfo);
                position.y = hitInfo.point.y + groundOffset;
                transform.position = position;
            }
        }

        private void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
