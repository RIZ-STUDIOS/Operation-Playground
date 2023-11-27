using OperationPlayground.EntityData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public class TankDestructionHandler : MonoBehaviour
    {
        [SerializeField] private GameObject tankAlivePrefab;
        [SerializeField] private GameObject tankDestroyedPrefab;
        [SerializeField] private ParticleSystem tankExplosionFX;

        private void Awake()
        {
            GetComponentInParent<GenericHealth>().OnDeath += DestroyTank;
        }

        public void DestroyTank()
        {
            transform.parent = null;

            tankAlivePrefab.SetActive(false);
            tankDestroyedPrefab.SetActive(true);
            tankExplosionFX.Play();

            StartCoroutine(DeathSequence());
        }

        private IEnumerator DeathSequence()
        {
            yield return new WaitForSeconds(3);

            Destroy(gameObject);
        }
    }
}
