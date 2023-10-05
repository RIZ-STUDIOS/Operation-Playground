using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RicTools;
using RicTools.Editor.Windows;
using UnityEditor;
using RicTools.Editor.Utilities;
using System;
using OperationPlayground.ScriptableObjects;

namespace OperationPlayground.Editor.Windows
{
    public class EnemyEditorWindow : GenericEditorWindow<EnemyScriptableObject, AvailableEnemyScriptableObject>
    {
        [SerializeField]
        private EditorContainer<GameObject> prefab = new EditorContainer<GameObject>();

        [SerializeField]
        private EditorContainer<int> health = new EditorContainer<int>(1);

        [SerializeField]
        private EditorContainer<float> attackRange = new EditorContainer<float>(1);

        [SerializeField]
        private EditorContainer<float> attackCooldown = new EditorContainer<float>(1);

        private SerializedProperty m_damageTypes;

        public DamageType[] damageTypes = new DamageType[] { };

        [MenuItem("Project Playground/Enemy Editor")]
    	public static EnemyEditorWindow ShowWindow()
        {
            return GetWindow<EnemyEditorWindow>("Enemy Editor");
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            m_damageTypes = serializedObject.FindProperty("damageTypes");
        }

        protected override void DrawGUI()
        {
            {
                var element = rootVisualElement.AddObjectField(prefab, "Prefab");

                RegisterCheckCompletion(element);
                RegisterLoadChange(element, prefab);
            }


            {
                var element = rootVisualElement.AddIntField(health, "Health");

                RegisterCheckCompletion(element);
                RegisterLoadChange(element, health);
            }

            rootVisualElement.AddTitle("Attack");

            {
                var element = rootVisualElement.AddFloatField(attackRange, "Attack Range");

                RegisterCheckCompletion(element);
                RegisterLoadChange(element, attackRange);
            }

            {
                var element = rootVisualElement.AddFloatField(attackCooldown, "Attack Cooldown");

                RegisterCheckCompletion(element);
                RegisterLoadChange(element, attackCooldown);
            }

            rootVisualElement.AddPropertyField(m_damageTypes, "Damage Types");
        }

        protected override void LoadScriptableObject(EnemyScriptableObject so, bool isNull)
        {
            if (isNull)
            {
                health.Value = 1;
                prefab.Value = null;
                damageTypes = new DamageType[] { };
                attackRange.Value = 1;
                attackCooldown.Value = 1;
            }
            else
            {
                health.Value = so.health;
                prefab.Value = so.prefab;
                damageTypes = so.damageTypes.Copy();
                attackRange.Value = so.attackRange;
                attackCooldown.Value = so.attackCooldown;
            }
        }

        protected override void CreateAsset(ref EnemyScriptableObject asset)
        {
            asset.health = health;
            asset.prefab = prefab;
            asset.damageTypes = damageTypes.Copy();
            asset.attackRange = attackRange;
            asset.attackCooldown = attackCooldown;
        }

        protected override IEnumerable<CompleteCriteria> GetCompleteCriteria()
        {
            yield return new CompleteCriteria(health.Value > 0, "Health must be above 0");
            yield return new CompleteCriteria(prefab.Value != null, "Prefab must not be null");
            yield return new CompleteCriteria(attackRange.Value > 0, "Attack Range must be above 0");
            yield return new CompleteCriteria(attackCooldown.Value > 0, "Attack Cooldown must be above 0");
        }
    }
}