using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RicTools;
using RicTools.Editor.Windows;
using UnityEditor;
using RicTools.Editor.Utilities;

namespace OperationPlayground
{
    public class EnemyEditorWindow : GenericEditorWindow<EnemyScriptableObject, AvailableEnemyScriptableObject>
    {
        [SerializeField]
        private EditorContainer<GameObject> prefab = new EditorContainer<GameObject>();

        [SerializeField]
        private EditorContainer<int> health = new EditorContainer<int>(1);

        [MenuItem("Window/RicTools Windows/EnemyEditorWindow")]
    	public static EnemyEditorWindow ShowWindow()
        {
            return GetWindow<EnemyEditorWindow>("EnemyEditorWindow");
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
        }

        protected override void LoadScriptableObject(EnemyScriptableObject so, bool isNull)
        {
            if (isNull)
            {
                health.Value = 1;
                prefab.Value = null;
            }
            else
            {
                health.Value = so.health;
                prefab.Value = so.prefab;
            }
        }

        protected override void CreateAsset(ref EnemyScriptableObject asset)
        {
            asset.health = health;
            asset.prefab = prefab;
        }

        protected override IEnumerable<CompleteCriteria> GetCompleteCriteria()
        {
            yield return new CompleteCriteria(health.Value > 0, "Health must be above 0");
            yield return new CompleteCriteria(prefab.Value != null, "Prefab must not be null");
        }
    }
}