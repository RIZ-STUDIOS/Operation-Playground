using OperationPlayground.Interactables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public class TestInteractable : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Interactable>().onInteract += () =>
            {
                Debug.LogWarning("Interacted");
            };
        }
    }
}
