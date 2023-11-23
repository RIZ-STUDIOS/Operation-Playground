using OperationPlayground.ScriptableObjects;
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
    public class RoundEditorWindow : GenericEditorWindow<RoundScriptableObject, AvailableRoundsScriptableObject>
    {
        public EditorContainer<int> minEnemies;
        public EditorContainer<int> maxEnemies;

        public EditorContainer<float> spawnDelay;

        public EditorContainer<int> supplyReward;

        public RoundEnemyData[] enemies;

        private SerializedProperty enemiesProperty;

        [MenuItem("Operation Playground/Round Editor")]
        public static RoundEditorWindow ShowWindow()
        {
            return GetWindow<RoundEditorWindow>("Round Editor");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            enemiesProperty = serializedObject.FindProperty(nameof(enemies));
        }

        protected override void DrawGUI()
        {
            rootVisualElement.AddLabel("Enemy Count");
            {
                var element = rootVisualElement.AddIntField(minEnemies, "Min");
                element.Q<Label>().style.paddingLeft = 15;
                RegisterLoadChange(element, minEnemies);
            }
            {
                var element = rootVisualElement.AddIntField(maxEnemies, "Max");
                element.Q<Label>().style.paddingLeft = 15;
                RegisterLoadChange(element, maxEnemies);
            }

            rootVisualElement.AddSeparator(new Color(0, 0, 0, 0));

            {
                var element = rootVisualElement.AddFloatField(spawnDelay, "Spawn Delay");
                RegisterLoadChange(element, spawnDelay);
            }

            {
                var element = rootVisualElement.AddIntField(supplyReward, "Supply Reward");
                RegisterLoadChange(element, supplyReward);
            }

            {
                var element = rootVisualElement.AddPropertyField(enemiesProperty, "Enemies");
                RegisterCheckCompletion(element);
            }
        }

        protected override void LoadScriptableObject(RoundScriptableObject so, bool isNull)
        {
            if (isNull)
            {
                minEnemies.Reset();
                maxEnemies.Reset();
                enemies = new RoundEnemyData[] { };
                spawnDelay.Reset();
                supplyReward.Reset();
            }
            else
            {
                minEnemies.Value = so.minEnemies;
                maxEnemies.Value = so.maxEnemies;
                enemies = so.enemies.Copy();
                spawnDelay.Value = so.spawnDelay;
                supplyReward.Value = so.supplyReward;
            }
        }

        protected override void CreateAsset(ref RoundScriptableObject asset)
        {
            asset.spawnDelay = spawnDelay;
            asset.enemies = enemies.Copy();
            asset.minEnemies = minEnemies;
            asset.maxEnemies = maxEnemies;
            asset.supplyReward = supplyReward;
        }

        protected override IEnumerable<CompleteCriteria> GetCompleteCriteria()
        {
            yield return new CompleteCriteria(enemies.Length > 0, "Need enemies");
        }
    }
}