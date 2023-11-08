using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RicTools;
using RicTools.Editor.Windows;
using UnityEditor;
using OperationPlayground.ScriptableObjects;
using RicTools.Editor.Utilities;

namespace OperationPlayground.Editor.Windows
{
    public class BuildingEditorWindow : GenericEditorWindow<BuildingScriptableObject, AvailableBuildingsScriptableObject>
    {
        public EditorContainer<GameObject> prefab;
        public EditorContainer<GameObject> visual;

        public EditorContainer<Vector3> timerOffset;
        public EditorContainer<float> placementDistance = new EditorContainer<float>(1);

        public EditorContainer<int> resourceCost;

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

            /*{
                var element = rootVisualElement.AddIntField(health, "Health");

                RegisterCheckCompletion(element);
                RegisterLoadChange(element, health);
            }*/

            {
                var element = rootVisualElement.AddFloatField(placementDistance, "Placement Distance");

                RegisterCheckCompletion(element);
                RegisterLoadChange(element, placementDistance);
            }

            {
                var element = rootVisualElement.AddIntField(resourceCost, "Resource Cost");

                RegisterCheckCompletion(element);
                RegisterLoadChange(element, resourceCost);
            }

            {
                var element = rootVisualElement.AddVector3Field(timerOffset, "Placement Distance");

                RegisterLoadChange(element, timerOffset);
            }
        }

        protected override void LoadScriptableObject(BuildingScriptableObject so, bool isNull)
        {
            if(isNull)
            {
                prefab.Reset();
                placementDistance.Reset();
                visual.Reset();
                timerOffset.Reset();
                resourceCost.Reset();
            }
            else
            {
                prefab.Value = so.prefab;
                placementDistance.Value = so.placementDistance;
                visual.Value = so.visual;
                timerOffset.Value = so.timerOffset;
                resourceCost.Value = so.resourceCost;
            }
        }

        protected override void CreateAsset(ref BuildingScriptableObject asset)
        {
            asset.prefab = prefab;
            asset.placementDistance = placementDistance;
            asset.visual = visual;
            asset.timerOffset = timerOffset;
            asset.resourceCost = resourceCost;

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
            yield return new CompleteCriteria(!prefab.IsNull(), "Needs a prefab");
            yield return new CompleteCriteria(!visual.IsNull(), "Needs a visual");
        }
    }
}