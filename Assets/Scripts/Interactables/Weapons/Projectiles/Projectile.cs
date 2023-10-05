using OperationPlayground.Enemy;
using OperationPlayground.ScriptableObjects;
using RicTools;
using RicTools.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Weapons.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        [System.NonSerialized]
        public ProjectileScriptableObject projectileSo;

        public GameObject parentShooter;

        private float groundOffset;
        private float timer;

        private void Start()
        {
            groundOffset = GetGroundOffset();
        }

        private void Update()
        {
            timer += Time.deltaTime;

            Move();

            if (timer >= projectileSo.travelDuration)
            {
                Destroy();
            }
        }

        private bool GetGroundHitInfo(out RaycastHit hitInfo)
        {
            return Physics.Raycast(transform.position, Vector3.down, out hitInfo, float.MaxValue, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore);
        }

        private float GetGroundOffset()
        {
            if (!GetGroundHitInfo(out var hitInfo))
            {
                return groundOffset;
            }
            return hitInfo.distance;
        }

        private void Move()
        {
            transform.position += transform.forward * projectileSo.speed * Time.deltaTime;

            var currentGroundOffset = GetGroundOffset();

            if (currentGroundOffset != groundOffset)
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

        private void OnTriggerEnter(Collider other)
        {
            if (parentShooter.tag == "Player")
            {
                if (other.tag == "Player") Destroy();
                else if (other.tag == "Enemy")
                {
                    EnemyHealth enemy = other.GetComponent<EnemyHealth>();
                    enemy.Damage(1);
                }
            }
            else if (parentShooter.tag == "Enemy")
            {
                if (other.tag == "Player")
                {
                    // Damage players.
                }
                else if (other.tag == "Enemy")
                {
                    Destroy();
                }
            }
            else Destroy();
        }
    }
}
