using OperationPlayground.Enemy;
using OperationPlayground.Managers;
using RicTools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OperationPlayground.UI
{
    public class ObjectHealthUI : MonoBehaviour
    {
        [SerializeField]
        private Slider healthSlider;

        [SerializeField]
        private Billboard billboard;

        [NonSerialized]
        public ObjectHealth parentHealth;

        private void Start()
        {
            billboard.LookAtTarget = GameManager.Instance.gameCamera.transform;

            UpdateHealthSlider();
            parentHealth.onHealthChange += UpdateHealthSlider;
        }

        private void UpdateHealthSlider()
        {
            healthSlider.value = parentHealth.HealthPer;
        }

        private void OnDestroy()
        {
            parentHealth.onHealthChange -= UpdateHealthSlider;
        }
    }
}
