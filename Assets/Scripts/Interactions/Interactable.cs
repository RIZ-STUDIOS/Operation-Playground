using OperationPlayground.Managers;
using OperationPlayground.Player;
using OperationPlayground.UI;
using RicTools.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OperationPlayground.Interactables
{
    [RequireComponent(typeof(SphereCollider))]
    public class Interactable : MonoBehaviour
    {
        [SerializeField, PositiveValueOnly]
        private float interactRadius;

        [SerializeField, PositiveValueOnly]
        private float interactionTime;

        [SerializeField]
        private bool destroyOnInteract;

        [SerializeField]
        private Vector3 radialBarOffset;

        [SerializeField]
        private bool _canInteractWith = true;

        public bool CanInteractWith { get { return _canInteractWith; } set { _canInteractWith = value; UpdateOutlineColors(); } }

        private SphereCollider sphereCollider;

        private List<Outline> outlines = new List<Outline>();

        public System.Action<PlayerManager> onInteract;

        private List<PlayerManager> nearbyPlayers = new List<PlayerManager>();

        private RadialBarUI radialBarUI;

        private PlayerManager currentPlayerInteracting;

        private float timer;

        private bool interactedWith;

        private void Awake()
        {
            sphereCollider = GetComponent<SphereCollider>();
            sphereCollider.isTrigger = true;

            var renderers = GetComponentsInChildren<Renderer>();

            foreach (var renderer in renderers)
            {
                var outline = renderer.gameObject.AddComponent<Outline>();
                if (!outline) continue;
                outline.enabled = false;
                outline.OutlineWidth = 7;
                outline.OutlineMode = Outline.Mode.OutlineVisible;
                outline.OutlineColor = Color.green;
                outlines.Add(outline);
            }

            var radialBarUIObject = Instantiate(PrefabsManager.Instance.data.radialBarUIPrefab);
            radialBarUIObject.transform.localScale = Vector3.one;
            radialBarUIObject.transform.parent = transform;
            radialBarUIObject.transform.localPosition = radialBarOffset;

            radialBarUI = radialBarUIObject.GetComponent<RadialBarUI>();

            UpdateRadialBar();

            CanInteractWith = _canInteractWith;
        }

        private void OnDestroy()
        {
            Destroy(sphereCollider);
            Destroy(radialBarUI);

            foreach (var outline in outlines)
            {
                Destroy(outline);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var playerManager = other.GetComponent<PlayerManager>();
            if (!playerManager) return;

            if (nearbyPlayers.Contains(playerManager)) return;

            playerManager.PlayerInteraction.SetInteractable(this);
        }

        private void OnTriggerExit(Collider other)
        {
            var playerManager = other.GetComponent<PlayerManager>();
            if (!playerManager) return;

            if (!nearbyPlayers.Contains(playerManager)) return;

            if (playerManager.PlayerInteraction.CurrentInteractable == this)
                playerManager.PlayerInteraction.SetInteractable(null);
        }

        private void OnValidate()
        {
            var collider = GetComponent<SphereCollider>();
            collider.radius = interactRadius;
            collider.isTrigger = true;
        }

        private void OnEnable()
        {
            UpdateOutlines();
            foreach(var playerManager in nearbyPlayers)
            {
                if(playerManager.PlayerInteraction.PreviousInteractable == this)
                    playerManager.PlayerInteraction.SetInteractable(this);
            }
        }

        private void OnDisable()
        {
            UpdateOutlines();
            foreach (var playerManager in nearbyPlayers)
            {
                if (playerManager.PlayerInteraction.CurrentInteractable == this)
                    playerManager.PlayerInteraction.SetInteractable(null);
            }
        }

        public void UpdateOutlines()
        {
            var enableOutlines = enabled && AnyPlayersNearby();

            foreach (var outline in outlines)
            {
                outline.enabled = enableOutlines;
            }
        }

        public bool AnyPlayersNearby()
        {
            return nearbyPlayers.Any(player => player.HasPlayerState(Player.PlayerCapabilities.PlayerCapabilityType.Interaction));
        }

        public void RemovePlayer(PlayerManager playerManager)
        {
            nearbyPlayers.Remove(playerManager);

            UpdateOutlines();
        }

        public void AddPlayer(PlayerManager playerManager)
        {
            nearbyPlayers.Add(playerManager);

            UpdateOutlines();
        }

        public void StartInteracting(PlayerManager playerManager)
        {
            currentPlayerInteracting = playerManager;
            UpdateRadialBar();
        }

        public void StopInteracting(PlayerManager playerManager)
        {
            if (currentPlayerInteracting == playerManager)
            {
                currentPlayerInteracting = null;
                timer = 0;
                interactedWith = false;
                UpdateRadialBar();
            }
        }

        private void UpdateRadialBar()
        {
            radialBarUI.gameObject.SetActive(currentPlayerInteracting);
            radialBarUI.PercentFilled = timer / interactionTime;
        }

        private void Update()
        {
            if (!CanInteractWith || !currentPlayerInteracting || interactedWith) return;

            timer += Time.deltaTime;
            UpdateRadialBar();

            if (timer >= interactionTime)
            {
                onInteract?.Invoke(currentPlayerInteracting);
                Debug.Log($"{currentPlayerInteracting} interacted with {gameObject}");
                interactedWith = true;
                StopInteracting(currentPlayerInteracting);
                if (destroyOnInteract)
                {
                    Destroy(this);
                }
            }
        }

        private void UpdateOutlineColors()
        {
            foreach (var outline in outlines)
            {
                outline.OutlineColor = _canInteractWith ? Color.green : Color.red;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawCube(transform.position + radialBarOffset, new Vector3(0.5f, 0.5f, 0.5f));
        }
    }
}
