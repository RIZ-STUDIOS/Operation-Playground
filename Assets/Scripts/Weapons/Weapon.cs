using OperationPlayground.Interactables;
using OperationPlayground.Player;
using OperationPlayground.Projectiles;
using OperationPlayground.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Weapons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField]
        private ProjectileScriptableObject projectileSo;

        public float reloadTime;

        public Vector3 offset;

        [SerializeField]
        private Transform fireTransform;

        [SerializeField]
        private bool infiniteAmmo;

        [SerializeField]
        private int ammoCount;

        [SerializeField]
        private Interactable interactable;

        private float cooldownTimer;

        public event System.Action onAmmoEnd;

        [System.NonSerialized]
        public ObjectHealth parentShooter;

        private void Awake()
        {
            if (interactable != null)
            {
                interactable.onInteract += Pickup;
            }
        }

        private void Pickup(PlayerManager playerManager)
        {
            if (playerManager.playerShooting.EquipWeapon(this))
            {
                Destroy(interactable);
                //Destroy(interactable.sphereCollider);
            }
        }

        public bool Shoot()
        {
            if (cooldownTimer <= 0 && (ammoCount > 0 || infiniteAmmo))
            {
                cooldownTimer = reloadTime;
                ShootProjectile();
                return true;
            }

            cooldownTimer -= Time.deltaTime;
            cooldownTimer = Mathf.Max(0, cooldownTimer);
            return false;
        }

        private void ShootProjectile()
        {
            var gameObject = Instantiate(projectileSo.prefab);

            gameObject.transform.position = fireTransform.position;
            gameObject.transform.forward = fireTransform.forward;

            var projectile = gameObject.AddComponent<Projectile>();
            projectile.projectileSo = projectileSo;
            projectile.parentShooter = parentShooter;
            if (!infiniteAmmo)
            {
                ammoCount--;

                if (ammoCount <= 0)
                {
                    onAmmoEnd?.Invoke();
                }
            }
        }
    }
}
