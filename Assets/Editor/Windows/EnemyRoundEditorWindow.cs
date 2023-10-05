using OperationPlayground.ScriptableObjects;
using RicTools;
using RicTools.Editor.Utilities;
using RicTools.Editor.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OperationPlayground.Editor.Windows
{
    public class EnemyRoundEditorWindow : GenericEditorWindow<EnemyRoundScriptableObject, AvailableEnemyRoundsScriptableObject>
    {
        public EnemyRoundData[] enemies = new EnemyRoundData[] { };
        public EnemyRoundData[] supportEnemies = new EnemyRoundData[] { };

        private SerializedProperty m_enemies;
        private SerializedProperty m_supportEnemies;

        [MenuItem("Operation Playground/Enemy Round Editor")]
        public static EnemyRoundEditorWindow ShowWindow()
        {
            return GetWindow<EnemyRoundEditorWindow>("Enemy Round Editor");
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            m_enemies = serializedObject.FindProperty("enemies");
            m_supportEnemies = serializedObject.FindProperty("supportEnemies");
        }

        protected override void DrawGUI()
        {
            rootVisualElement.AddPropertyField(m_enemies, "Enemies");
            rootVisualElement.AddPropertyField(m_supportEnemies, "Support Enemies");
        }

        protected override void LoadScriptableObject(EnemyRoundScriptableObject so, bool isNull)
        {
            if (isNull)
            {
                enemies = new EnemyRoundData[] { };
                supportEnemies = new EnemyRoundData[] { };
            }
            else
            {
                enemies = so.enemies.Copy();
                supportEnemies = so.supportEnemies.Copy();
            }
        }

        protected override void CreateAsset(ref EnemyRoundScriptableObject asset)
        {
            asset.enemies = enemies.Copy();
            asset.supportEnemies = supportEnemies.Copy();
        }
    }
}