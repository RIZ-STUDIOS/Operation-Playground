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
        private const int MAX_AUDIO_SOURCES = 10;

        public WeaponScriptableObject weaponSo;

        [SerializeField]
        private Transform _firePointTransform;
        public Transform FirePointTransform { get { return _firePointTransform; } }

        [SerializeField]
        protected bool infiniteAmmo;

        public bool InfiniteAmmo { get { return infiniteAmmo; } set { infiniteAmmo = value; } }

        protected int currentAmmo;

        public int CurrentAmmo => currentAmmo;

        protected float shootCooldown;

        protected GenericShooter shooter;

        public System.Action onAmmoChange;

        public AudioSource GunshotSound { get { return gunshotSound; } }
        protected AudioSource gunshotSound;

        private List<AudioSource> audioSources = new List<AudioSource>();

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
            gunshotSound = GetComponentInChildren<AudioSource>();
        }

        private void Start()
        {
            currentAmmo = weaponSo.maxAmmo;
            onAmmoChange?.Invoke();
            gunshotSound.clip = weaponSo.gunshotAudioclip;
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

            ShootGun();

            return true;
        }

        protected virtual void ShootGun()
        {
            shootCooldown = weaponSo.cooldown;

            var projectileObject = Projectile.CreateProjectile(weaponSo.projectileScriptableObject, shooter);

            projectileObject.transform.position = _firePointTransform.position;
            projectileObject.transform.LookAt(GetFirePointForwardVector());
            projectileObject.transform.forward = GetFirePointForwardVector();

            if (!infiniteAmmo)
            {
                currentAmmo--;
            }
            onAmmoChange?.Invoke();

            if (weaponSo.gunshotAudioclip != null)
            {
                var gunshotClone = Instantiate(gunshotSound);
                gunshotClone.transform.position = _firePointTransform.position;
                gunshotClone.Play();
                Destroy(gunshotClone.gameObject, gunshotClone.clip.length);
                audioSources.RemoveAll(x => x == null);
                while(audioSources.Count > MAX_AUDIO_SOURCES)
                {
                    var source = audioSources[0];
                    Destroy(source.gameObject);
                    audioSources.RemoveAt(0);
                }
                audioSources.Add(gunshotClone);
            }
        }

        protected virtual Vector3 GetFirePointForwardVector()
        {
            var deviation = Random.insideUnitCircle * weaponSo.deviationOffsetModifier;
            return (_firePointTransform.forward + new Vector3(deviation.x, deviation.y)).normalized;
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
            onAmmoChange?.Invoke();
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
    }
}
