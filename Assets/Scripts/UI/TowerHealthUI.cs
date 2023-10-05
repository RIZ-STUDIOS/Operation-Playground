using OperationPlayground.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OperationPlayground.UI
{
    public class TowerHealthUI : MonoBehaviour
    {
        [SerializeField]
        private Slider healthSlider;

        private void Start()
        {
            UpdateHealthSlider();

            GameManager.Instance.towerHealth.onHealthChange += UpdateHealthSlider;
        }

        private void UpdateHealthSlider()
        {
            healthSlider.value = GameManager.Instance.towerHealth.HealthPer;
        }
    }
}
