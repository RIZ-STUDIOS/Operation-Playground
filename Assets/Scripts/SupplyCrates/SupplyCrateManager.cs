using OperationPlayground.Managers;
using RicTools.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.SupplyCreates
{
    public class SupplyCrateManager : MonoBehaviour
    {
        [MinValue(0.001f)]
        public float timeToDescend = 1;

        public SupplyCreateSpawnLocation[] locations;

        private void Awake()
        {
            GameManager.Instance.supplyCrateManager = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                SpawnRandomCrate();
            }
        }

        public void SpawnRandomCrate()
        {
            foreach (var location in locations)
            {
                if (location.TrySpawnCrate()) break;
            }
        }
    }
}
