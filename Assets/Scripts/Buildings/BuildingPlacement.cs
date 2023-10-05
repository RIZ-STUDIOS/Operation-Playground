using OperationPlayground.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public class BuildingPlacement : MonoBehaviour
    {
        public Building toPlace;

        private Renderer[] meshRenderers;

        private Dictionary<Renderer, Material> materials = new Dictionary<Renderer, Material>();

        private bool canPlace, materialCanPlace;

        private void Awake()
        {
            meshRenderers = GetComponentsInChildren<Renderer>();

            foreach (var renderer in meshRenderers)
            {
                var material = renderer.material;
                materials.Add(renderer, material);
            }
        }

        public void StartPlacement()
        {
            foreach (var renderer in meshRenderers)
            {
                renderer.material = MaterialsManager.Instance.data.placementMaterial;
            }
            CheckPlacement();
            toPlace.StartPlacement();
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

        private void CheckPlacement()
        {
            canPlace = CheckCanPlace();
            if (canPlace && !materialCanPlace)
            {
                GreenMaterial();
            }else if(!canPlace && materialCanPlace)
            {
                RedMaterial();
            }
        }

        public bool Place()
        {
            if (!canPlace) return false;
            foreach(var material in materials)
            {
                material.Key.material = material.Value;
            }

            transform.SetParent(null);

            toPlace.Place();
            Destroy(this);
            return true;
        }

        private bool CheckCanPlace()
        {
            return Physics.OverlapBox(transform.position, toPlace.buildingSo.boundsToCheck / 2f, transform.rotation, LayerMask.GetMask("EnemyPath")).Length == 0;
        }
    }
}
