using OperationPlayground.Interactables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [System.NonSerialized]
        public Interactable interactable;

        private PlayerManager playerInputManager;

        private List<Interactable> interactables = new List<Interactable>();

        private void Awake()
        {
            playerInputManager = GetComponent<PlayerManager>();
        }

        public void EnableInteraction()
        {
            playerInputManager.playerInput.Player.Interact.performed += OnInteractPerformed;

            foreach (var interactable in interactables)
            {
                interactable.UpdateOutlines();
            }
        }

        public void DisableInteraction()
        {
            playerInputManager.playerInput.Player.Interact.performed -= OnInteractPerformed;

            foreach (var interactable in interactables)
            {
                interactable.UpdateOutlines();
            }
        }

        private void OnInteractPerformed(InputAction.CallbackContext callbackContext)
        {
            var value = callbackContext.ReadValue<Vector2>();

            var button = GetInteractionButton(value);

            foreach (var interactable in interactables)
            {
                if (interactable.button == button)
                {
                    interactable.onInteract?.Invoke(playerInputManager);
                    break;
                }
            }
        }

        public void AddInteractable(Interactable interactable)
        {
            if (interactables.Contains(interactable)) return;
            interactables.Add(interactable);
        }

        public void RemoveInteractable(Interactable interactable)
        {
            if (!interactables.Contains(interactable)) return;
            interactables.Remove(interactable);
        }

        public static InteractionButton GetInteractionButton(Vector2 value)
        {
            if (value.x == 1)
                return InteractionButton.Right;
            else if (value.x == -1)
                return InteractionButton.Left;

            if (value.y == 1)
                return InteractionButton.Top;
            else if (value.y == -1)
                return InteractionButton.Bottom;

            return InteractionButton.Bottom;
        }
    }
}
