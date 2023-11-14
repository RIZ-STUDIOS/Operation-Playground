using OperationPlayground.ScriptableObjects;
using OperationPlayground.ScriptableObjects.Projectiles;
using RicTools;
using RicTools.Editor.Utilities;
using RicTools.Editor.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace OperationPlayground.Editor.Windows
{
    public class EnemyEditorWindow : GenericEditorWindow<EnemyScriptableObject, AvailableEnemiesScriptableObject>
    {
        public EditorContainer<GameObject> prefab;
        public EditorContainer<float> speed;
        public EditorContainer<int> maxHealth;
        public EditorContainer<WeaponScriptableObject> weaponScriptableObject;

        public DamageType[] damageTypes;

        private SerializedProperty damageTypesProperty;

        [MenuItem("Operation Playground/Enemy Editor")]
        public static EnemyEditorWindow ShowWindow()
        {
            return GetWindow<EnemyEditorWindow>("Enemy Editor");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            damageTypesProperty = serializedObject.FindProperty("damageTypes");
        }

        protected override void DrawGUI()
        {
            {
                var element = rootVisualElement.AddObjectField(prefab, "Prefab");
                RegisterLoadChange(element, prefab);
                RegisterCheckCompletion(element);
            }

            {
                var element = rootVisualElement.AddIntField(maxHealth, "Max Health");
                RegisterLoadChange(element, maxHealth);
            }

            {
                var element = rootVisualElement.AddFloatField(speed, "Speed");
                RegisterLoadChange(element, speed);
            }

            {
                var element = rootVisualElement.AddObjectField(weaponScriptableObject, "Weapon");
                RegisterLoadChange(element, weaponScriptableObject);
            }

            {
                var element = rootVisualElement.AddPropertyField(damageTypesProperty, "Damage Types");
            }
        }

        protected override void LoadScriptableObject(EnemyScriptableObject so, bool isNull)
        {
            if (isNull)
            {
                maxHealth.Reset();
                weaponScriptableObject.Reset();
                prefab.Reset();
                damageTypes = new DamageType[] { };
                speed.Reset();
            }
            else
            {
                maxHealth.Value = so.maxHealth;
                weaponScriptableObject.Value = so.weaponScriptableObject;
                prefab.Value = so.prefab;
                damageTypes = so.vulnerableDamageTypes.Copy();
                speed.Value = so.speed;
            }
        }

        protected override void CreateAsset(ref EnemyScriptableObject asset)
        {
            asset.weaponScriptableObject = weaponScriptableObject;
            asset.maxHealth = maxHealth;
            asset.prefab = prefab;
            asset.vulnerableDamageTypes = damageTypes.Copy();
            asset.speed = speed;
        }

        protected override IEnumerable<CompleteCriteria> GetCompleteCriteria()
        {
            yield return new CompleteCriteria(!prefab.IsNull(), "Prefab cannot be null");
        }
    }
}