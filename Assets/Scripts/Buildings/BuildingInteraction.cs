using OperationPlayground.Interactables;
using OperationPlayground.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Buildings
{
    public class BuildingInteraction : MonoBehaviour
    {
        private Interactable interactable;

        private ObjectHealth objectHealth;

        private List<PlayerInteraction> players = new List<PlayerInteraction>();

        public event System.Action<GameObject> onEnterBuilding;
        public event System.Action<GameObject> onExitBuilding;

        protected virtual void Awake()
        {
            interactable = GetComponent<Interactable>();
            interactable.onPlayerNearby += OnPlayerNearby;
            interactable.onInteract += OnInteract;
            interactable.canInteract += CanInteract;
            objectHealth = GetComponent<ObjectHealth>();
            objectHealth.onDeath += OnDeath;
        }

        private void OnPlayerNearby(GameObject playerGameObject)
        {
            interactable.SetOutlineColor(CanEnter() ? Color.green : Color.red);
        }

        protected virtual bool CanInteract(GameObject playerGameObject)
        {
            return !players.Contains(playerGameObject.GetComponent<PlayerInteraction>());
        }

        protected virtual bool CanEnter()
        {
            return players.Count <= 0;
        }

        private void OnInteract(GameObject playerGameObject)
        {
            if (!CanEnter()) return;
            EnterBuilding(playerGameObject);
        }

        public void EnterBuilding(GameObject playerGameObject)
        {
            var playerInteraction = playerGameObject.GetComponentInChildren<PlayerInteraction>();
            players.Add(playerInteraction);
            playerInteraction.GetComponent<PlayerInputManager>().DisablePlayer();
            onEnterBuilding?.Invoke(playerGameObject);
        }

        public void ExitBuilding(GameObject playerGameObject)
        {
            var playerInteraction = playerGameObject.GetComponentInChildren<PlayerInteraction>();
            players.Remove(playerInteraction);
            playerInteraction.GetComponent<PlayerInputManager>().EnablePlayer();
            onExitBuilding?.Invoke(playerGameObject);
        }

        private void OnDeath()
        {
            foreach (var player in players)
            {
                ExitBuilding(player.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
