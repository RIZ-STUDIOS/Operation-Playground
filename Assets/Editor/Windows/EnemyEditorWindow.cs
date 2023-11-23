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
        public EditorContainer<Sprite> enemySprite = new EditorContainer<Sprite>();
        public EditorContainer<float> attackRange;
        public EditorContainer<float> targetCheckTime;
        public EditorContainer<float> attackDelayTime;
        public EditorContainer<float> shootingTime;

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
                var element = rootVisualElement.AddObjectField(enemySprite, "Sprite");
                RegisterLoadChange(element, enemySprite);
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
                var element = rootVisualElement.AddFloatField(attackRange, "Attack Range");
                RegisterLoadChange(element, attackRange);
            }

            {
                var element = rootVisualElement.AddFloatField(targetCheckTime, "Attack Check Time");
                RegisterLoadChange(element, targetCheckTime);
            }

            {
                var element = rootVisualElement.AddFloatField(attackDelayTime, "Attack Delay Time");
                RegisterLoadChange(element, attackDelayTime);
            }

            {
                var element = rootVisualElement.AddFloatField(shootingTime, "Shooting Time");
                RegisterLoadChange(element, shootingTime);
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
                enemySprite.Reset();
                attackRange.Reset();
                targetCheckTime.Reset();
                attackDelayTime.Reset();
                shootingTime.Reset();
            }
            else
            {
                maxHealth.Value = so.maxHealth;
                weaponScriptableObject.Value = so.weaponScriptableObject;
                prefab.Value = so.prefab;
                damageTypes = so.vulnerableDamageTypes.Copy();
                speed.Value = so.speed;
                enemySprite.Value = so.enemySprite;
                attackRange.Value = so.attackRange;
                targetCheckTime.Value = so.targetCheckTime;
                attackDelayTime.Value = so.attackDelayTime;
                shootingTime.Value = so.shootingTime;
            }
        }

        protected override void CreateAsset(ref EnemyScriptableObject asset)
        {
            asset.weaponScriptableObject = weaponScriptableObject;
            asset.maxHealth = maxHealth;
            asset.prefab = prefab;
            asset.vulnerableDamageTypes = damageTypes.Copy();
            asset.speed = speed;
            asset.enemySprite = enemySprite;
            asset.attackRange = attackRange;
            asset.attackDelayTime = attackDelayTime;
            asset.targetCheckTime = targetCheckTime;
            asset.shootingTime = shootingTime;
        }

        protected override IEnumerable<CompleteCriteria> GetCompleteCriteria()
        {
            yield return new CompleteCriteria(!prefab.IsNull(), "Prefab cannot be null");
        }
    }
}