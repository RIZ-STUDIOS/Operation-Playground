using OperationPlayground.Interactables;
using OperationPlayground.Player;
using OperationPlayground.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.SupplyDrop
{
    public class SupplyCrate : MonoBehaviour
    {
        [System.NonSerialized]
        public Interactable interactable;

        [System.NonSerialized]
        public int supplyAmount;

        [SerializeField]
        private GameObject[] parachuteGameObjects;

        private void Awake()
        {
            interactable = GetComponent<Interactable>();

            interactable.onInteract += OnInteract;
        }

        public void OnLand()
        {
            interactable.enabled = true;

            foreach(var obj in parachuteGameObjects)
            {
                obj.SetActive(false);
            }
        }

        private void OnInteract(PlayerManager playerManager)
        {
            ResourceManager.Instance.Supplies += supplyAmount;
        }
    }
}
