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
    public class WeaponEditorWindow : GenericEditorWindow<WeaponScriptableObject, AvailableWeaponsScriptableObject>
    {
        public EditorContainer<GameObject> prefab = new EditorContainer<GameObject>();
        public EditorContainer<float> cooldown = new EditorContainer<float>(1);

        [MenuItem("Operation Playground/Weapon Editor")]
    	public static WeaponEditorWindow ShowWindow()
        {
            return GetWindow<WeaponEditorWindow>("Weapon Editor");
        }

        protected override void DrawGUI()
        {
            {
                var element = rootVisualElement.AddObjectField(prefab, "Prefab");
                RegisterLoadChange(element, prefab);
                RegisterCheckCompletion(element);
            }

            {
                var element = rootVisualElement.AddFloatField(cooldown, "Cooldown");
                RegisterLoadChange(element, cooldown);
                RegisterCheckCompletion(element);
            }
        }

        protected override void LoadScriptableObject(WeaponScriptableObject so, bool isNull)
        {
            if(isNull)
            {
                cooldown.Reset();
                prefab.Reset();
            }
            else
            {
                cooldown.Value = so.cooldown;
                prefab.Value = so.prefab;
            }
        }

        protected override void CreateAsset(ref WeaponScriptableObject asset)
        {
            asset.cooldown = cooldown;
            asset.prefab = prefab;
        }

        protected override IEnumerable<CompleteCriteria> GetCompleteCriteria()
        {
            yield return new CompleteCriteria(!prefab.IsNull(), "Need prefab");
            yield return new CompleteCriteria(cooldown > 0, "Cooldown must be higher than 0");
        }
    }
}