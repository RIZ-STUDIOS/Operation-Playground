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

        private PlayerInputManager playerInputManager;

        private void Awake()
        {
            playerInputManager = GetComponent<PlayerInputManager>();
        }

        private void OnEnable()
        {
            EnableInput();
            if (interactable != null)
            {
                interactable.AddPlayer(this, true);
            }
        }

        private void OnDisable()
        {
            DisableInput();
            if (interactable != null)
            {
                interactable.RemovePlayer(this, true);
            }
        }

        private void EnableInput()
        {
            playerInputManager.playerInput.Player.Interact.performed += OnInteractPerformed;
        }

        private void DisableInput()
        {
            playerInputManager.playerInput.Player.Interact.performed -= OnInteractPerformed;
        }

        private void OnInteractPerformed(InputAction.CallbackContext value)
        {
            if (!interactable) return;

            var vector = value.ReadValue<Vector2>();

            var button = GetInteractButton(vector);

            if (button == interactable.interactButton)
            {
                interactable.Interact(gameObject);
            }
        }

        private InteractButton GetInteractButton(Vector2 data)
        {
            if (data.x == 1)
            {
                return InteractButton.Right;
            }
            else if (data.x == -1)
            {
                return InteractButton.Left;
            }

            if (data.y == 1)
            {
                return InteractButton.Top;
            }
            else if (data.y == -1)
            {
                return InteractButton.Bottom;
            }

            return InteractButton.None;
        }
    }
}
