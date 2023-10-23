using OperationPlayground.Managers;
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
        private float waterStrength;

        [SerializeField]
        private float waterDamage;

        private GameObject waterBeamPrefab;

        private ParticleSystem currentBeam;

        [System.NonSerialized]
        public ObjectHealth parentShooter;

        private void Awake()
        {
            waterBeamPrefab = PrefabsManager.Instance.data.waterBeamPrefab;

            var beamGameObject = Instantiate(waterBeamPrefab);
            beamGameObject.transform.SetParent(shootTransform, false);
            beamGameObject.transform.localPosition = Vector3.zero;

            currentBeam = beamGameObject.GetComponent<ParticleSystem>();

            var waterBeam = beamGameObject.GetComponent<WaterBeam>();
            waterBeam.SetBeamStrength(waterStrength);

            var damageParticles = beamGameObject.GetComponent<DamageParticles>();
            damageParticles.damagePerSecond = waterDamage;
            damageParticles.shooter = parentShooter;

            StopShooting();
        }

        public void StartShoot()
        {
            currentBeam.Play();
        }

        public void StopShooting()
        {
            if (!currentBeam) return;

            currentBeam.Stop();
        }
    }
}
