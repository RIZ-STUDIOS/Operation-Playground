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
    public class ProjectileEditorWindow : GenericEditorWindow<ProjectileScriptableObject, AvailableProjectilesScriptableObject>
    {
        [SerializeField]
        private EditorContainer<GameObject> prefab = new EditorContainer<GameObject>();

        [SerializeField]
        private EditorContainer<float> travelDuration = new EditorContainer<float>(1);

        [SerializeField]
        private EditorContainer<float> speed = new EditorContainer<float>(1);

        [MenuItem("Operation Playground/Projectile Editor")]
    	public static ProjectileEditorWindow ShowWindow()
        {
            return GetWindow<ProjectileEditorWindow>("Projectile Editor");
        }

        protected override void DrawGUI()
        {
            {
                var element = rootVisualElement.AddObjectField(prefab, "Prefab");

                RegisterCheckCompletion(element);
                RegisterLoadChange(element, prefab);
            }

            {
                var element = rootVisualElement.AddFloatField(travelDuration, "Travel Duration");

                RegisterCheckCompletion(element);
                RegisterLoadChange(element, travelDuration);
            }

            {
                var element = rootVisualElement.AddFloatField(speed, "Speed");

                RegisterCheckCompletion(element);
                RegisterLoadChange(element, speed);
            }
        }

        protected override void LoadScriptableObject(ProjectileScriptableObject so, bool isNull)
        {
            if(isNull)
            {
                prefab.Value = null;
                travelDuration.Value = 1;
                speed.Value = 1;
            }
            else
            {
                prefab.Value = so.prefab;
                travelDuration.Value = so.travelDuration;
                speed.Value = so.speed;
            }
        }

        protected override void CreateAsset(ref ProjectileScriptableObject asset)
        {
            asset.prefab = prefab;
            asset.travelDuration = travelDuration;
            asset.speed = speed;
        }
    }
}