using RicTools.ScriptableObjects;
using RicTools.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace RicTools.Editor.CustomEditors
{
    [UnityEditor.CustomEditor(typeof(AvailableScriptableObject<>), true)]
    [CanEditMultipleObjects]
    public class AvailableScriptableObjectCustomEditor : UnityEditor.Editor
    {
        private static bool listOpened;

        public override void OnInspectorGUI()
        {
            var availableScriptableObjectType = target.GetType();
            var availableScriptableObject = target;
            var itemsField = availableScriptableObjectType.GetField("items");
            var itemsArray = (GenericScriptableObject[])itemsField.GetValue(availableScriptableObject);

            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((ScriptableObject)target), typeof(AvailableScriptableObject<>), false);
            GUI.enabled = true;

            EditorGUILayout.BeginHorizontal();
            listOpened = EditorGUILayout.BeginFoldoutHeaderGroup(listOpened, "Items");
            GUI.enabled = false;
            EditorGUILayout.IntField(itemsArray.Length, GUILayout.Width(50));
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
            if (listOpened)
            {
                GUI.enabled = false;
                foreach (var item in itemsArray)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (item)
                    {
                        EditorGUILayout.LabelField(item.id, GUILayout.MaxWidth(125));
                        EditorGUILayout.ObjectField(item, item.GetType(), false);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                GUI.enabled = true;
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Delete All"))
            {
                List<int> toRemove = new List<int>();
                int i = 0;
                foreach (var item in itemsArray)
                {
                    if (!AssetDatabase.Contains(item))
                    {
                        toRemove.Add(i - toRemove.Count);
                    }
                    else
                    {
                        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath((Object)item));
                    }
                    i++;
                }
                AssetDatabase.SaveAssets();
                if (toRemove.Count > 0)
                {
                    var items = (IList)System.Activator.CreateInstance(typeof(List<>).MakeGenericType(itemsArray[0].GetType()));
                    foreach (var item in itemsArray)
                    {
                        items.Add(item);
                    }
                    foreach (var j in toRemove)
                    {
                        items.RemoveAt(j);
                    }
                    availableScriptableObjectType.GetMethodRecursive("SetItems", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(availableScriptableObject, new object[] { items });

                    EditorUtility.SetDirty(availableScriptableObject);
                    AssetDatabase.SaveAssets();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
