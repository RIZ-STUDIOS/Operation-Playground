using OperationPlayground.Player;
using OperationPlayground.Player.PlayerStates;
using RicTools.Attributes;
using RicTools.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

namespace OperationPlayground.Interactables
{
    public enum InteractionButton
    {
        Bottom,
        Top,
        Left,
        Right
    }

    [RequireComponent(typeof(SphereCollider))]
    public class Interactable : MonoBehaviour
    {
        private SphereCollider sphereCollider;

        [SerializeField, PositiveValueOnly]
        private float interactRadius;

        public InteractionButton button;

        public System.Action<PlayerManager> onInteract;

        private List<PlayerManager> nearbyPlayers = new List<PlayerManager>();

        private List<Outline> outlines = new List<Outline>();

        private void Awake()
        {
            sphereCollider = gameObject.GetOrAddComponent<SphereCollider>();
            sphereCollider.radius = interactRadius;
            sphereCollider.isTrigger = true;

            var meshRenderers = GetComponentsInChildren<MeshRenderer>();

            foreach (var renderer in meshRenderers)
            {
                var outline = renderer.gameObject.AddComponent<Outline>();
                outline.enabled = false;
                outline.OutlineWidth = 7;
                outline.OutlineMode = Outline.Mode.OutlineVisible;
                outline.OutlineColor = Color.green;
                outlines.Add(outline);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var playerManager = other.GetComponentInParent<PlayerManager>();
            if (!playerManager) return;

            if (PlayerNearby(playerManager)) return;

            nearbyPlayers.Add(playerManager);

            if (enabled)
            {
                playerManager.playerInteraction.AddInteractable(this);
                UpdateOutlines();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var playerManager = other.GetComponentInParent<PlayerManager>();
            if (!playerManager) return;

            if (!PlayerNearby(playerManager)) return;

            playerManager.playerInteraction.RemoveInteractable(this);

            nearbyPlayers.Remove(playerManager);

            if (enabled)
            {
                UpdateOutlines();
            }
        }

        private void OnEnable()
        {
            UpdateOutlines();
            foreach (var player in nearbyPlayers)
            {
                player.playerInteraction.AddInteractable(this);
            }
        }

        private void OnDisable()
        {
            DisableOutlines();
            RemoveAllPlayers();
        }

        private bool PlayerNearby(PlayerManager playerManager)
        {
            return nearbyPlayers.Contains(playerManager);
        }

        public bool AnyPlayerNearby()
        {
            return nearbyPlayers.Any(p => p.HasPlayerState(PlayerStateType.Interaction));
        }

        public void UpdateOutlines()
        {
            var playersNearby = AnyPlayerNearby();

            CheckOutlines();
            foreach (var outline in outlines)
            {
                outline.enabled = playersNearby;
            }
        }

        private void DisableOutlines()
        {
            CheckOutlines();
            foreach (var outline in outlines)
            {
                outline.enabled = false;
            }
        }

        private void RemoveAllPlayers()
        {
            foreach (var player in nearbyPlayers)
            {
                player.playerInteraction.RemoveInteractable(this);
            }
        }

        private void OnDestroy()
        {
            Destroy(sphereCollider);
            RemoveAllPlayers();
        }

        private void OnValidate()
        {
            var collider = GetComponent<SphereCollider>();
            collider.radius = interactRadius;
        }

        private void CheckOutlines()
        {
            outlines.RemoveAll(o => o == null);
        }
    }
}
