using System.Collections;
using UnityEngine;

namespace OperationPlayground.EntityData
{
    public class EntityRagdollHandler : MonoBehaviour
    {
        private Rigidbody[] rigidbodies;

        private void Awake()
        {
            rigidbodies = GetComponentsInChildren<Rigidbody>();
            DisableRagdoll();

            GetComponentInParent<GenericHealth>().OnDeath += EnableRagdoll;
        }

        public void DisableRagdoll()
        {
            foreach (var rigidbody in rigidbodies)
            {
                rigidbody.isKinematic = true;
            }
        }

        public void EnableRagdoll()
        {
            transform.parent = null;

            GetComponent<Animator>().enabled = false;

            foreach (var rigidbody in rigidbodies)
            {
                rigidbody.isKinematic = false;
            }

            StartCoroutine(DeathSequence());
        }

        private IEnumerator DeathSequence()
        {
            yield return new WaitForSeconds(3);

            Destroy(gameObject);
        }
    }
}