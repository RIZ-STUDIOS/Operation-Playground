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

        [SerializeField]
        private ParticleSystem landingParticles;

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

            landingParticles.transform.localPosition = new Vector3(randomisedPosition.x, 0.01f, randomisedPosition.y);
            landingParticles.Play();
            StartCoroutine(MoveCrateDown());
        }

        private IEnumerator MoveCrateDown()
        {
            var position = currentSupplyCrate.transform.position;

            var speed = spawnHeight / GameManager.Instance.supplyCrateManager.timeToDescend;

            while (position.y > transform.position.y)
            {
                position.y -= speed * Time.deltaTime;
                currentSupplyCrate.transform.position = position;
                yield return null;
            }

            position.y = transform.position.y;
            currentSupplyCrate.transform.position = position;

            currentSupplyCrate.onLand?.Invoke();
            landingParticles.Stop();
            landingParticles.time = 0;
        }
    }
}
