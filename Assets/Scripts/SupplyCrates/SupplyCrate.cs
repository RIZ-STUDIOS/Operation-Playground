using OperationPlayground.Interactables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.SupplyCreates
{
    public class SupplyCrate : MonoBehaviour
    {
        public System.Action onLand;

        [System.NonSerialized]
        public Interactable interactable;

        private void Awake()
        {
            interactable = GetComponent<Interactable>();

            onLand += () =>
            {
                interactable.enabled = true;
            };
        }
    }
}
