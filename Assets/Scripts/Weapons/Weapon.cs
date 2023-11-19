using OperationPlayground.EntityData;
using OperationPlayground.Interactables;
using OperationPlayground.Player;
using OperationPlayground.Projectiles;
using OperationPlayground.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace OperationPlayground.Weapons
{
    public class Weapon : MonoBehaviour
    {
        public WeaponScriptableObject weaponSo;

        [System.NonSerialized]
        public Interactable interactable;

        [SerializeField]
        private Transform _firePointTransform;
        public Transform FirePointTransform { get { return _firePointTransform; } }

        [SerializeField]
        private bool infiniteAmmo;

        private int currentAmmo;

        private float shootCooldown;

        private GenericShooter shooter;

        public static GameObject CreateWeapon(WeaponScriptableObject weaponScriptableObject, Transform parentTransform = null)
        {
            GameObject weaponObject = Instantiate(weaponScriptableObject.prefab);
            var weapon = weaponObject.GetComponent<Weapon>();
            if (!weapon) throw new System.Exception($"No Weapon component in weapon's prefab '{weaponScriptableObject.id}'");

            weaponObject.transform.SetParent(parentTransform, true);

            weapon.weaponSo = weaponScriptableObject;

            weapon.ApplyOffset();

            return weaponObject;
        }

        private void Awake()
        {
            interactable = GetComponent<Interactable>();

            if (interactable)
            {
                interactable.onInteract += OnInteract;
            }
        }

        private void Start()
        {
            currentAmmo = weaponSo.maxAmmo;
        }

        private void Update()
        {
            shootCooldown -= Time.deltaTime;

            if (shootCooldown < 0)
            {
                shootCooldown = 0;
            }
        }

        public bool Shoot()
        {
            if (!shooter) return false;
            if (!HasAmmo()) return false;
            if (shootCooldown > 0) return false;

            shootCooldown = weaponSo.cooldown;

            var projectileObject = Projectile.CreateProjectile(weaponSo.projectileScriptableObject, shooter);

            projectileObject.transform.position = _firePointTransform.position;
            projectileObject.transform.forward = _firePointTransform.forward;

            if (!infiniteAmmo)
            {
                currentAmmo--;
            }

            return true;
        }

        public void ApplyOffset()
        {
            transform.localPosition = weaponSo.slotOffset;
        }

        public bool HasAmmo()
        {
            return currentAmmo > 0 || infiniteAmmo;
        }

        public int AddAmmo(int amount)
        {
            if (currentAmmo >= weaponSo.maxAmmo) return amount;
            currentAmmo += amount;
            var diff = 0;
            if (currentAmmo > weaponSo.maxAmmo)
            {
                diff = currentAmmo - weaponSo.maxAmmo;
                currentAmmo = weaponSo.maxAmmo;
            }
            return diff;
        }

        public void SetShooter(GenericShooter shooter)
        {
            this.shooter = shooter;        
        }

        public bool CompareScriptableObject(WeaponScriptableObject weaponScriptableObject)
        {
            return weaponScriptableObject == weaponSo;
        }

        private void OnInteract(PlayerManager playerManager)
        {
            playerManager.PlayerShooter.AddWeapon(this);
        }
    }
}
