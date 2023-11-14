using OperationPlayground.Interactables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        public System.Action<Interactable> onSetInteractable;

        private PlayerManager playerManager;

        private Interactable currentInteractable;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();

            playerManager.playerInput.Interaction.Interact.performed += OnInteractPerformed;
            playerManager.playerInput.Interaction.Interact.canceled += OnInteractCanceled;
        }

        public void SetInteractable(Interactable interactable)
        {
            if (currentInteractable)
            {
                currentInteractable.RemovePlayer(playerManager);
            }
            currentInteractable = interactable;
            if (interactable)
                currentInteractable.AddPlayer(playerManager);

            onSetInteractable?.Invoke(interactable);
        }

        public void UpdateInteractable()
        {
            if (!currentInteractable) return;
            currentInteractable.UpdateOutlines();
        }

        private void OnInteractPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (!currentInteractable) return;
            currentInteractable.StartInteracting(playerManager);
        }

        private void OnInteractCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (!currentInteractable) return;
            currentInteractable.StopInteracting(playerManager);
        }
    }
}
