using OperationPlayground.Managers;
using OperationPlayground.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OperationPlayground.Buildings
{
    public class BuildingPlacement : MonoBehaviour
    {
        public BuildingHealth toPlace;

        private Renderer[] meshRenderers;

        private Dictionary<Renderer, Material> materials = new Dictionary<Renderer, Material>();

        [System.NonSerialized]
        public PlayerManager playerPlacing;

        private Collider[] colliders;

        private bool canPlace, materialCanPlace;

        private void Awake()
        {
            meshRenderers = GetComponentsInChildren<Renderer>();

            foreach (var renderer in meshRenderers)
            {
                var material = renderer.material;
                materials.Add(renderer, material);
            }

            colliders = GetComponentsInChildren<Collider>();
            foreach (var collider in colliders)
            {
                collider.enabled = false;
            }
        }

        public void StartPlacement()
        {
            foreach (var renderer in meshRenderers)
            {
                renderer.material = MaterialsManager.Instance.data.placementMaterial;
            }
            CheckPlacement(true);
        }

        private void GreenMaterial()
        {
            foreach (var renderer in meshRenderers)
            {
                var material = renderer.material;

                material.color = new Color(0, 1, 0, 0.25f);

                renderer.material = material;
            }
            materialCanPlace = true;
        }

        private void RedMaterial()
        {
            foreach (var renderer in meshRenderers)
            {
                var material = renderer.material;

                material.color = new Color(1, 0, 0, 0.25f);

                renderer.material = material;
            }
            materialCanPlace = false;
        }

        private void Update()
        {
            CheckPlacement();
        }

        private void CheckPlacement(bool force = false)
        {
            canPlace = CheckCanPlace();
            if (canPlace && (!materialCanPlace || force))
            {
                GreenMaterial();
            }
            else if (!canPlace && (materialCanPlace || force))
            {
                RedMaterial();
            }
        }

        public bool Place()
        {
            if (!canPlace) return false;
            foreach (var material in materials)
            {
                material.Key.material = material.Value;
            }

            foreach (var collider in colliders)
            {
                collider.enabled = true;
            }

            transform.SetParent(null);

            gameObject.AddComponent<InvalidPlacement>();

            toPlace.Place();
            Destroy(this);
            return true;
        }

        private bool CheckCanPlace()
        {
            var colliders = Physics.OverlapBox(transform.position, toPlace.buildingSo.boundsToCheck / 2f, transform.rotation, Physics.AllLayers, QueryTriggerInteraction.Ignore);

            var invalidPlacements = colliders.Select(c => c.GetComponentInParent<InvalidPlacement>()).ToList().FindAll(c => c != null && c.invalid);

            var playerIndex = invalidPlacements.FindIndex(c => c.GetComponent<PlayerManager>() == playerPlacing);

            if (playerIndex >= 0)
                invalidPlacements.RemoveAt(playerIndex);

            return invalidPlacements.Count == 0;
        }
    }
}
