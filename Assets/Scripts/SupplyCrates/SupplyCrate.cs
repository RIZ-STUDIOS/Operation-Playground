using OperationPlayground.Buildings;
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

        private InvalidPlacement invalidPlacement;
        private EnemyTarget target;

        private void Awake()
        {
            interactable = GetComponent<Interactable>();
            target = GetComponent<EnemyTarget>();
            invalidPlacement = GetComponent<InvalidPlacement>();

            onLand += () =>
            {
                interactable.enabled = true;
                target.visible = true;
            };
        }
    }
}
