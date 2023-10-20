using OperationPlayground.Managers;
using OperationPlayground.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public class WaterGunWeapon : MonoBehaviour
    {
        [SerializeField]
        private Transform shootTransform;

        [SerializeField]
        private WaterProjectileScriptableObject projectileSo;

        private GameObject waterBeamPrefab;

        private WaterBeam waterBeam;

        [System.NonSerialized]
        public ObjectHealth parentShooter;

        private bool triggerDown;
        private bool canShoot;

        private float currentCooldown;

        private void Awake()
        {
            waterBeamPrefab = PrefabsManager.Instance.data.waterBeamPrefab;

            var beamGameObject = Instantiate(waterBeamPrefab);
            beamGameObject.transform.SetParent(shootTransform, false);
            beamGameObject.transform.localPosition = Vector3.zero;

            waterBeam = beamGameObject.GetComponent<WaterBeam>();
            waterBeam.SetBeamStrength(projectileSo.beamStrength);
            waterBeam.SetBeamDuration(projectileSo.beamDuration);

            waterBeam.onParticleSystemStopped += () =>
            {
                if (projectileSo.IsSingleFire) return;

                currentCooldown = projectileSo.beamCooldown;
                canShoot = false;
            };

            canShoot = true;

            var damageParticles = beamGameObject.GetComponent<DamageParticles>();
            damageParticles.damagePerSecond = projectileSo.damagePerSecond;
            damageParticles.shooter = parentShooter;

            StopShooting();
        }

        public void StartShoot()
        {
            waterBeam.Play();
            triggerDown = true;
        }

        public void StopShooting()
        {
            waterBeam.Stop();
            triggerDown = false;
        }

        private void Update()
        {
            if(triggerDown && !canShoot && currentCooldown <= 0)
            {
                Debug.Log("shoot");
                canShoot = true;
                waterBeam.Play();
            }

            currentCooldown -= Time.deltaTime;
            currentCooldown = Mathf.Max(currentCooldown, 0);
        }
    }
}
