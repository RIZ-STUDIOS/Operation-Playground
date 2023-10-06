using OperationPlayground.Player;
using RicTools.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace OperationPlayground.Interactables
{
    public enum InteractButton
    {
        Top,
        Bottom,
        Left,
        Right,
        None
    }

    [RequireComponent(typeof(SphereCollider))]
    public class Interactable : MonoBehaviour
    {
        [SerializeField, PositiveValueOnly]
        private float interactDistance;

        public InteractButton interactButton = InteractButton.Bottom;

        [System.NonSerialized]
        public SphereCollider sphereCollider;

        private List<Outline> outlines = new List<Outline>();

        private List<PlayerInteraction> players = new List<PlayerInteraction>();

        public event System.Action<GameObject> onInteract;
        public event System.Func<GameObject, bool> canInteract;
        public event System.Action<GameObject> onPlayerNearby;

        private void Awake()
        {
            sphereCollider = GetComponent<SphereCollider>();
            sphereCollider.radius = interactDistance;
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

        private void OnEnable()
        {
            foreach (var player in players)
            {
                AddPlayer(player);
            }
            UpdateOutline();
        }

        private void OnDisable()
        {
            foreach (var player in players)
            {
                RemovePlayer(player);
            }
            HideOutline();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawWireSphere(transform.position, interactDistance);
        }

        private void OnTriggerEnter(Collider other)
        {
            var playerInteraction = other.GetComponentInParent<PlayerInteraction>();
            if (!playerInteraction) return;
            players.Add(playerInteraction);
            if (!enabled) return;
            AddPlayer(playerInteraction);
        }

        private void OnTriggerExit(Collider other)
        {
            var playerInteraction = other.GetComponentInParent<PlayerInteraction>();
            if (!playerInteraction) return;
            players.Remove(playerInteraction);
            if (!enabled) return;
            RemovePlayer(playerInteraction);
        }

        public void RemovePlayer(PlayerInteraction playerInteraction, bool removeReference = false)
        {
            if (playerInteraction.interactable == this)
                playerInteraction.interactable = null;
            if (removeReference)
                players.Remove(playerInteraction);
            UpdateOutline();
        }

        public void AddPlayer(PlayerInteraction playerInteraction, bool addReference = false)
        {
            if (canInteract != null)
            {
                if (!canInteract.Invoke(playerInteraction.gameObject)) return;
            }
            playerInteraction.interactable = this;
            onPlayerNearby?.Invoke(playerInteraction.gameObject);
            if (addReference && !players.Contains(playerInteraction))
                players.Add(playerInteraction);
            UpdateOutline();
        }

        private void UpdateOutline()
        {
            if (players.Count > 0 && enabled)
            {
                ShowOutline();
            }
            else
            {
                HideOutline();
            }
        }

        private void ShowOutline()
        {
            foreach (Outline outline in outlines)
            {
                outline.enabled = true;
            }
        }

        public void SetOutlineColor(Color color)
        {
            foreach (var outline in outlines)
            {
                outline.OutlineColor = color;
            }
            UpdateOutline();
        }

        private void HideOutline()
        {
            foreach (Outline outline in outlines)
            {
                outline.enabled = false;
            }
        }

        private void OnDestroy()
        {
            foreach (var player in players)
            {
                if (player.interactable == this)
                    player.interactable = null;
            }
            HideOutline();
            foreach (var outline in outlines)
            {
                Destroy(outline);
            }
        }

        public void Interact(GameObject gameObject)
        {
            onInteract?.Invoke(gameObject);
        }
    }
}
