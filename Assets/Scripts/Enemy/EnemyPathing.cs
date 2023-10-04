using OperationPlayground.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

namespace OperationPlayground.Enemy
{
    public class EnemyPathing : MonoBehaviour
    {
        private SplineAnimate splineAnimate;
        private EnemyHealth enemyHealth;

        private void Awake()
        {
            splineAnimate = GetComponent<SplineAnimate>();
            enemyHealth = GetComponent<EnemyHealth>();
        }

        private void Update()
        {
            if(splineAnimate.NormalizedTime >= 1)
            {
                OnEndReached();
            }
        }

        private void OnEndReached()
        {
            GameManager.Instance.towerHealth.Damage(enemyHealth.Health);
            Destroy(gameObject);
        }
    }
}
