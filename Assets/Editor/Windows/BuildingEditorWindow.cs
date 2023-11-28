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
        public EditorContainer<GameObject> prefab;
        public EditorContainer<GameObject> visual;

        public EditorContainer<Vector3> timerOffset;
        public EditorContainer<Vector3> placementDistance = new EditorContainer<Vector3>(new Vector3(0,0,1));

        public EditorContainer<int> health = new EditorContainer<int>(1);

        public EditorContainer<int> resourceCost;
        public EditorContainer<Sprite> buildingSprite = new EditorContainer<Sprite>();

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
                var element = rootVisualElement.AddObjectField(visual, "Visual");

                RegisterCheckCompletion(element);
                RegisterLoadChange(element, visual);
            }

            {
                var element = rootVisualElement.AddObjectField(buildingSprite, "Sprite");

                RegisterCheckCompletion(element);
                RegisterLoadChange(element, buildingSprite);
            }

            {
                var element = rootVisualElement.AddIntField(health, "Health");

                RegisterCheckCompletion(element);
                RegisterLoadChange(element, health);
            }

            {
                var element = rootVisualElement.AddVector3Field(placementDistance, "Placement Offset");

                RegisterCheckCompletion(element);
                RegisterLoadChange(element, placementDistance);
            }

            {
                var element = rootVisualElement.AddIntField(resourceCost, "Resource Cost");

                RegisterCheckCompletion(element);
                RegisterLoadChange(element, resourceCost);
            }

            {
                var element = rootVisualElement.AddVector3Field(timerOffset, "Timer Offset");

                RegisterLoadChange(element, timerOffset);
            }
        }

        protected override void LoadScriptableObject(BuildingScriptableObject so, bool isNull)
        {
            if (isNull)
            {
                prefab.Reset();
                placementDistance.Reset();
                visual.Reset();
                timerOffset.Reset();
                resourceCost.Reset();
                health.Reset();
                buildingSprite.Reset();
            }
            else
            {
                prefab.Value = so.prefab;
                placementDistance.Value = so.placementOffset;
                visual.Value = so.visual;
                timerOffset.Value = so.timerOffset;
                resourceCost.Value = so.resourceCost;
                health.Value = so.health;
                buildingSprite.Value = so.buildingSprite;
            }
        }

        protected override void CreateAsset(ref BuildingScriptableObject asset)
        {
            asset.prefab = prefab;
            asset.placementOffset = placementDistance;
            asset.visual = visual;
            asset.timerOffset = timerOffset;
            asset.resourceCost = resourceCost;
            asset.health = health;
            asset.buildingSprite = buildingSprite;

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
            asset.boundsToCheck = bounds.size;

            DestroyImmediate(gameObject);
        }

        protected override IEnumerable<CompleteCriteria> GetCompleteCriteria()
        {
            yield return new CompleteCriteria(!prefab.IsNull(), "Needs a prefab");
            yield return new CompleteCriteria(!visual.IsNull(), "Needs a visual");
        }
    }
}