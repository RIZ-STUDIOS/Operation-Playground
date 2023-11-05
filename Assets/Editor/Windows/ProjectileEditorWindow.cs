using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RicTools;
using RicTools.Editor.Windows;
using UnityEditor;
using OperationPlayground.ScriptableObjects;
using OperationPlayground.ScriptableObjects.Projectiles;
using RicTools.Editor.Utilities;
using UnityEngine.UIElements;

namespace OperationPlayground.Editor.Windows
{
    public class ProjectileEditorWindow : GenericEditorWindow<ProjectileScriptableObject, AvailableProjectilesScriptableObject>
    {
        public EditorContainer<GameObject> prefab;
        public EditorContainer<ProjectileType> projectileType;
        public EditorContainer<float> maxRange;
        public EditorContainer<float> speed;
        public EditorContainer<float> aliveTime;
        public EditorContainer<DamageType> damageType;

        private FloatField maxRangeFloatField;

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
                var element = rootVisualElement.AddEnumField(projectileType, "Projectile Type", UpdateVisibleFields);
                RegisterLoadChange(element, projectileType);
            }

            {
                var element = rootVisualElement.AddFloatField(speed, "Speed");
                RegisterLoadChange(element, speed);
            }

            {
                var element = rootVisualElement.AddFloatField(aliveTime, "Time Alive");
                RegisterLoadChange(element, aliveTime);
            }

            {
                maxRangeFloatField = rootVisualElement.AddFloatField(maxRange, "Max Range");
                RegisterLoadChange(maxRangeFloatField, maxRange);
            }

            {
                var element = rootVisualElement.AddEnumField(damageType, "Damage Type");
                RegisterLoadChange(element, damageType);
            }

            UpdateVisibleFields();
        }

        protected override void LoadScriptableObject(ProjectileScriptableObject so, bool isNull)
        {
            if(isNull)
            {
                prefab.Reset();
                projectileType.Reset();
            }
            else
            {
                prefab.Value = so.prefab;
                projectileType.Value = so.projectileType;
            }

            UpdateVisibleFields();
        }

        protected override void CreateAsset(ref ProjectileScriptableObject asset)
        {
            if(projectileType == ProjectileType.Arc)
            {
                var projectileAsset = ScriptableObject.CreateInstance<ArcProjectileScriptableObject>();

                asset = projectileAsset;
            }

            asset.prefab = prefab;
            asset.projectileType = projectileType;
        }

        private void UpdateVisibleFields()
        {
            maxRangeFloatField.ToggleClass("hidden", projectileType != ProjectileType.Arc);
        }

        protected override IEnumerable<CompleteCriteria> GetCompleteCriteria()
        {
            yield return new CompleteCriteria(!prefab.IsNull(), "Need prefab");
        }
    }
}