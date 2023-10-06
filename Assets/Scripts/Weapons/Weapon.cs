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

        [SerializeField]
        private float reloadTime;

        public Vector3 offset;

        [SerializeField]
        private Transform fireTransform;

        [SerializeField]
        private bool infiniteAmmo;

        [SerializeField]
        private int ammoCount;

        private Interactable interactable;

        private float cooldownTimer;

        public event System.Action onAmmoEnd;

        [System.NonSerialized]
        public ObjectHealth parentShooter;

        private void Awake()
        {
            interactable = GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.onInteract += Pickup;
            }
        }

        private void Pickup(GameObject playerGameObject)
        {
            if (playerGameObject.GetComponent<PlayerShooting>().EquipWeapon(this))
            {
                Destroy(interactable);
                Destroy(interactable.sphereCollider);
            }
        }

        public void Shoot()
        {
            if (cooldownTimer <= 0 && (ammoCount > 0 || infiniteAmmo))
            {
                cooldownTimer = reloadTime;
                ShootProjectile();
            }

            cooldownTimer -= Time.deltaTime;
            cooldownTimer = Mathf.Max(0, cooldownTimer);
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
