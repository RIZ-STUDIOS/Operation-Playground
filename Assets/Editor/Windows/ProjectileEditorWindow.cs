using OperationPlayground.ScriptableObjects;
using OperationPlayground.ScriptableObjects.Projectiles;
using RicTools;
using RicTools.Editor.Utilities;
using RicTools.Editor.Windows;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
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
        public EditorContainer<int> damage;

        private FloatField maxRangeFloatField;
        private FloatField timeAliveFloatField;

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
                timeAliveFloatField = rootVisualElement.AddFloatField(aliveTime, "Time Alive");
                RegisterLoadChange(timeAliveFloatField, aliveTime);
            }

            {
                maxRangeFloatField = rootVisualElement.AddFloatField(maxRange, "Max Range");
                RegisterLoadChange(maxRangeFloatField, maxRange);
            }

            {
                var element = rootVisualElement.AddIntField(damage, "Damage");
                RegisterLoadChange(element, damage);
            }

            UpdateVisibleFields();
        }

        protected override void LoadScriptableObject(ProjectileScriptableObject so, bool isNull)
        {
            if (isNull)
            {
                prefab.Reset();
                projectileType.Reset();
                damage.Reset();
                maxRange.Reset();
                speed.Reset();
                aliveTime.Reset();
            }
            else
            {
                prefab.Value = so.prefab;
                projectileType.Value = so.projectileType;
                damage.Value = so.damage;
                speed.Value = so.speed;
                if (so is ArcProjectileScriptableObject arcProj)
                {
                    maxRange.Value = arcProj.maxRange;
                }
                else if (so is StraightProjectileScriptableObject straightProj)
                {
                    aliveTime.Value = straightProj.aliveTime;
                }
            }

            UpdateVisibleFields();
        }

        protected override void CreateAsset(ref ProjectileScriptableObject asset)
        {
            if (projectileType == ProjectileType.Arc)
            {
                if (asset is not ArcProjectileScriptableObject projectileAsset)
                    projectileAsset = ScriptableObject.CreateInstance<ArcProjectileScriptableObject>();

                projectileAsset.maxRange = maxRange;

                asset = projectileAsset;
            }
            else
            {
                if (asset is not StraightProjectileScriptableObject projectileAsset)
                    projectileAsset = ScriptableObject.CreateInstance<StraightProjectileScriptableObject>();

                projectileAsset.aliveTime = aliveTime;

                asset = projectileAsset;
            }

            asset.prefab = prefab;
            asset.projectileType = projectileType;
            asset.damage = damage;
            asset.speed = speed;
        }

        private void UpdateVisibleFields()
        {
            maxRangeFloatField.ToggleClass("hidden", projectileType != ProjectileType.Arc);
            timeAliveFloatField.ToggleClass("hidden", projectileType != ProjectileType.Straight);
        }

        protected override IEnumerable<CompleteCriteria> GetCompleteCriteria()
        {
            yield return new CompleteCriteria(!prefab.IsNull(), "Need prefab");
        }
    }
}