using RicTools.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    [RequireComponent(typeof(ParticleSystem))]
    public class WaterBeam : MonoBehaviour
    {
        [SerializeField, MinValue(0.001f)]
        private float beamStrength;

        [SerializeField, MustBeAssigned]
        private ParticleSystem waterBeamParticles;

        private float currentBeamStrength;

        private void Awake()
        {
            SetBeamStrength(beamStrength);
        }

        public void SetBeamStrength(float beamStrength)
        {
            currentBeamStrength = beamStrength;
            UpdateParticlesSpeed();
        }

        private void UpdateParticlesSpeed()
        {
            var main = waterBeamParticles.main;
            main.startSpeed = currentBeamStrength;
            var emission = waterBeamParticles.emission;
            emission.rateOverTime = currentBeamStrength*2;
        }

        private void OnValidate()
        {
            var main = waterBeamParticles.main;
            main.startSpeed = beamStrength;
            var emission = waterBeamParticles.emission;
            emission.rateOverTime = beamStrength * 2;
        }
    }
}
