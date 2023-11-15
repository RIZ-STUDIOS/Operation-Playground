using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OperationPlayground.Managers;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace OperationPlayground.SupplyDrop
{
    public class SupplyDropLocation : MonoBehaviour
    {
        [SerializeField]
        private float crateHeightSpawn;

        [SerializeField]
        private float crateSpawnRange;

        private SupplyCrate supplyCrate;

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            Handles.color = Color.cyan;
            Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, crateSpawnRange);
            Handles.DrawWireArc(transform.position + (Vector3.up * crateHeightSpawn), Vector3.up, Vector3.forward, 360, crateSpawnRange);
            Handles.DrawLine(transform.position, transform.position + (Vector3.up * crateHeightSpawn));
#endif
        }

        public void SpawnCrate()
        {
            if (supplyCrate) return;

            var crateGameObject = Instantiate(PrefabsManager.Instance.data.cratePrefab);

            crateGameObject.transform.position = RandomSpawnPoint();

            supplyCrate = crateGameObject.GetComponent<SupplyCrate>();

            supplyCrate.interactable.onInteract += (playerManager) =>
            {
                supplyCrate = null;
            };
        }

        private Vector3 RandomSpawnPoint()
        {
            var position = transform.position + (Vector3.up * crateHeightSpawn);

            var randomCirclePosition = Random.insideUnitCircle * crateSpawnRange;

            position += new Vector3(randomCirclePosition.x, 0, randomCirclePosition.y);

            return position;
        }

        public bool HasCrate()
        {
            return supplyCrate;
        }

        private void Update()
        {
            if (!supplyCrate) return;
            var position = supplyCrate.transform.position;

            if (position.y <= transform.position.y) return;

            position.y -= GameManager.Instance.supplyDropManager.dropSpeed * Time.deltaTime;

            if (position.y < transform.position.y)
            {
                position.y = transform.position.y;
            }

            supplyCrate.transform.position = position;

            if (position.y <= transform.position.y)
            {
                supplyCrate.OnLand();
            }
        }
    }
}
