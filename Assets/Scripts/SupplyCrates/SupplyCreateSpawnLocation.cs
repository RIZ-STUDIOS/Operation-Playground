using OperationPlayground.Managers;
using RicTools.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.SupplyCreates
{
    public class SupplyCreateSpawnLocation : MonoBehaviour
    {
        [MinValue(0.001f)]
        public float range = 1;

        [MinValue(0.001f)]
        public float spawnHeight = 10;

        private GameObject supplyCratePrefab;

        private SupplyCrate currentSupplyCrate;

        private void Awake()
        {
            supplyCratePrefab = PrefabsManager.Instance.data.supplyCratePrefab;
        }

        public bool TrySpawnCrate()
        {
            if (currentSupplyCrate) return false;
            SpawnCrate();
            return true;
        }

        private void SpawnCrate()
        {
            var supplyCrateGameObject = Instantiate(supplyCratePrefab);
            var position = new Vector3(0, spawnHeight, 0);
            var randomisedPosition = Random.insideUnitCircle * range;
            position += new Vector3(randomisedPosition.x, 0, randomisedPosition.y);
            supplyCrateGameObject.transform.position = transform.position + position;

            currentSupplyCrate = supplyCrateGameObject.GetComponent<SupplyCrate>();

            currentSupplyCrate.interactable.onInteract += (player) =>
            {
                currentSupplyCrate = null;
                Destroy(supplyCrateGameObject);
            };

            StartCoroutine(MoveCrateDown());
        }

        private IEnumerator MoveCrateDown()
        {
            var position = currentSupplyCrate.transform.position;

            while (position.y > transform.position.y)
            {
                position.y -= GameManager.Instance.supplyCrateManager.descentVelocity * Time.deltaTime;
                currentSupplyCrate.transform.position = position;
                yield return null;
            }

            position.y = transform.position.y;
            currentSupplyCrate.transform.position = position;

            currentSupplyCrate.onLand?.Invoke();
        }
    }
}
