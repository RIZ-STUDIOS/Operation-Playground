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

        private WaterBeam currentBeam;

        [System.NonSerialized]
        public ObjectHealth parentShooter;

        private void Awake()
        {
            waterBeamPrefab = PrefabsManager.Instance.data.waterBeamPrefab;
        }

        public void StartShoot()
        {
            Debug.Log("start");

            StopShooting();

            var beamGameObject = Instantiate(waterBeamPrefab);
            beamGameObject.transform.SetParent(shootTransform, false);
            beamGameObject.transform.localPosition = Vector3.zero;

            currentBeam = beamGameObject.GetComponent<WaterBeam>();
            currentBeam.SetBeamStrength(waterStrength);

            var damageParticles = beamGameObject.GetComponent<DamageParticles>();
            damageParticles.damagePerSecond = waterDamage;
            damageParticles.shooter = parentShooter;
        }

        public void StopShooting()
        {
            if (!currentBeam) return;
            Debug.Log("stop");

            currentBeam.GetComponent<ParticleSystem>().Stop();
            currentBeam = null;
        }
    }
}
