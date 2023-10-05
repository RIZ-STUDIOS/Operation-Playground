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

        private SphereCollider sphereCollider;

        private List<Outline> outlines = new List<Outline>();

        private List<PlayerInteraction> players = new List<PlayerInteraction>();

        public event System.Action onInteract;

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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawWireSphere(transform.position, interactDistance);
        }

        private void OnTriggerEnter(Collider other)
        {
            var playerInteraction = other.GetComponentInParent<PlayerInteraction>();
            if (!playerInteraction) return;
            playerInteraction.interactable = this;
            players.Add(playerInteraction);
            UpdateOutline();
        }

        private void OnTriggerExit(Collider other)
        {
            var playerInteraction = other.GetComponentInParent<PlayerInteraction>();
            if (!playerInteraction) return;
            if (playerInteraction.interactable == this)
                playerInteraction.interactable = null;
            players.Remove(playerInteraction);
            UpdateOutline();
        }

        private void UpdateOutline()
        {
            if (players.Count > 0)
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

        private void HideOutline()
        {
            foreach (Outline outline in outlines)
            {
                outline.enabled = false;
            }
        }

        private void OnDestroy()
        {
            foreach(var player in players)
            {
                if(player.interactable == this)
                player.interactable = null;
            }
        }

        public void Interact()
        {
            onInteract?.Invoke();
        }
    }
}
