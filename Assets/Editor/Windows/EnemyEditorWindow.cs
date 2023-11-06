using OperationPlayground.ScriptableObjects;
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
    public class EnemyEditorWindow : GenericEditorWindow<EnemyScriptableObject, AvailableEnemiesScriptableObject>
    {
        public EditorContainer<int> maxHealth;
        public EditorContainer<WeaponScriptableObject> weaponScriptableObject;

        [MenuItem("Operation Playground/Enemy Editor")]
        public static EnemyEditorWindow ShowWindow()
        {
            return GetWindow<EnemyEditorWindow>("Enemy Editor");
        }

        protected override void DrawGUI()
        {
            {
                var element = rootVisualElement.AddIntField(maxHealth, "Max Health");
                RegisterLoadChange(element, maxHealth);
            }

            {
                var element = rootVisualElement.AddObjectField(weaponScriptableObject, "Weapon");
                RegisterLoadChange(element, weaponScriptableObject);
            }
        }

        protected override void LoadScriptableObject(EnemyScriptableObject so, bool isNull)
        {
            if (isNull)
            {
                maxHealth.Reset();
                weaponScriptableObject.Reset();
            }
            else
            {
                maxHealth.Value = so.maxHealth;
                weaponScriptableObject.Value = so.weaponScriptableObject;
            }
        }

        protected override void CreateAsset(ref EnemyScriptableObject asset)
        {
            asset.weaponScriptableObject = weaponScriptableObject;
            asset.maxHealth = maxHealth;
        }
    }
}