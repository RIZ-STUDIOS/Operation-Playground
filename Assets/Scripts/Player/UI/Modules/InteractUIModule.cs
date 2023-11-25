using OperationPlayground.Interactables;
using OperationPlayground.ZedExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.UI.Modules
{
    public class InteractUIModule : UIModule
    {
        protected override void Awake()
        {
            base.Awake();

            _playerCanvas.playerManager.PlayerInteraction.onSetInteractable += OnSetInteractable;
        }

        private void OnSetInteractable(Interactable interactable)
        {
            if (interactable && interactable.enabled)
                FadeRevealModule();
            else
                FadeHideModule();
        }

        public void EnableInteract()
        {
            FadeRevealModule();
        }

        public void DisableInteract(PlayerManager playerManager)
        {
            FadeHideModule();
        }
    }
}
