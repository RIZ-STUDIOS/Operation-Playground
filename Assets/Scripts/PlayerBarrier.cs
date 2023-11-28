using OperationPlayground.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public class PlayerBarrier : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var enemy = other.GetComponentInParent<EnemyEntity>();
            if (!enemy) return;
            enemy.EnemyHealth.spawnAmmo = true;
        }
    }
}
