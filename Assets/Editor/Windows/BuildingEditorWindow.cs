using OperationPlayground.ScriptableObjects;
using RicTools;
using RicTools.Editor.Utilities;
using RicTools.Editor.Windows;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OperationPlayground.Editor.Windows
{
    public class BuildingEditorWindow : GenericEditorWindow<BuildingScriptableObject, AvailableBuildingsScriptableObject>
    {
        [SerializeField]
        private EditorContainer<GameObject> prefab = new EditorContainer<GameObject>();

        [SerializeField]
        private EditorContainer<float> health = new EditorContainer<float>(1);

        [SerializeField]
        private EditorContainer<BuildingType> buildingType = new EditorContainer<BuildingType>();

        [SerializeField]
        private EditorContainer<float> placementDistance = new EditorContainer<float>(1);

        [MenuItem("Operation Playground/Building Editor")]
        public static BuildingEditorWindow ShowWindow()
        {
            return GetWindow<BuildingEditorWindow>("Building Editor");
        }

        protected override void DrawGUI()
        {
            {
                var element = rootVisualElement.AddObjectField(prefab, "Prefab");

                RegisterCheckCompletion(element);
                RegisterLoadChange(element, prefab);
            }

            {
                var element = rootVisualElement.AddFloatField(health, "Health");

                RegisterCheckCompletion(element);
                RegisterLoadChange(element, health);
            }

            {
                var element = rootVisualElement.AddFloatField(placementDistance, "Placement Distance");

                RegisterCheckCompletion(element);
                RegisterLoadChange(element, placementDistance);
            }
        }

        protected override void LoadScriptableObject(BuildingScriptableObject so, bool isNull)
        {
            if (isNull)
            {
                prefab.Value = null;
                health.Value = 1;
                placementDistance.Value = 1;
                buildingType.Value = default;
            }
            else
            {
                prefab.Value = so.prefab;
                health.Value = so.health;
                placementDistance.Value = so.placementDistance;
                buildingType.Value = so.buildingType;
            }
        }

        protected override void CreateAsset(ref BuildingScriptableObject asset)
        {
            asset.prefab = prefab;
            asset.health = health;
            asset.placementDistance = placementDistance;
            asset.buildingType = buildingType;

            var gameObject = new GameObject();
            var prefabCreated = GameObject.Instantiate(prefab.Value);
            prefabCreated.transform.parent = gameObject.transform;

            var combinedMesh = new Mesh();
            MeshFilter[] meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];
            for (int i = 0; i < meshFilters.Length; i++)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            }
            combinedMesh.CombineMeshes(combine);
            var bounds = combinedMesh.bounds;
            asset.boundsToCheck = bounds.size * 1.5f;

            DestroyImmediate(gameObject);
        }

        protected override IEnumerable<CompleteCriteria> GetCompleteCriteria()
        {
            yield return new CompleteCriteria(health.Value > 0, "Health must be above 0");
            yield return new CompleteCriteria(prefab.Value != null, "Prefab must not be null");
            yield return new CompleteCriteria(placementDistance.Value > 0, "Placement Distance must be above 0");
        }
    }
}