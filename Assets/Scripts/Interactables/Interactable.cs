using RicTools.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    [RequireComponent(typeof(SphereCollider))]
    public class Interactable : MonoBehaviour
    {
        [SerializeField, PositiveValueOnly]
        private float interactDistance;

        private SphereCollider sphereCollider;

        private int playersIn;

        private List<Outline> outlines = new List<Outline>();

        private void Awake()
        {
            sphereCollider = GetComponent<SphereCollider>();
            sphereCollider.radius = interactDistance;
            sphereCollider.isTrigger = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawWireSphere(transform.position, interactDistance);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
            playersIn++;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag != "Player") return;
            playersIn--;
        }

        private void UpdateOutline()
        {

        }
    }
}
