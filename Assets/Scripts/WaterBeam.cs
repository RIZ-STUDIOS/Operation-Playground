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

        public event System.Action onParticleSystemStopped;

        private void Awake()
        {
            SetBeamStrength(beamStrength);
        }

        public void SetBeamStrength(float beamStrength)
        {
            this.beamStrength = beamStrength;
            UpdateParticlesSpeed();
        }

        public void SetBeamDuration(float beamDuration)
        {
            var main = waterBeamParticles.main;
            main.loop = beamDuration <= 0;
            main.duration = beamDuration <= 0 ? 100 : beamDuration;
        }

        private void UpdateParticlesSpeed()
        {
            var main = waterBeamParticles.main;
            main.startSpeed = beamStrength;
            var emission = waterBeamParticles.emission;
            emission.rateOverTime = beamStrength*2;
        }

        private void OnValidate()
        {
            UpdateParticlesSpeed();
        }

        public void Play()
        {
            waterBeamParticles.Play();
        }

        public void Stop()
        {
            waterBeamParticles.Stop();
        }

        private void OnParticleSystemStopped()
        {
            onParticleSystemStopped?.Invoke();
        }
    }
}
