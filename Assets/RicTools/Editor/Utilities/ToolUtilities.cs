using RicTools.Editor.Settings;
using RicTools.Editor.Windows;
using RicTools.ScriptableObjects;
using RicTools.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace RicTools.Editor.Utilities
{
    public static class ToolUtilities
    {
        public static List<T> FindAssetsByType<T>() where T : Object
        {
            List<T> assets = new List<T>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }

        internal static bool HasCustomEditor(System.Type type)
        {
            return GetCustomEditorType(type) != null;
        }

        internal static System.Type GetCustomEditorType(System.Type type)
        {
            var editorTypes = GetCustomEditorTypes();
            foreach (var editorType in editorTypes)
            {
                var genericTypeArguments = editorType.GetTypeInfo().BaseType.GetTypeInfo().GenericTypeArguments;
                if (genericTypeArguments.Length == 0) continue;
                if (genericTypeArguments[0] == type) return editorType;
            }

            return null;
        }

        internal static bool HasAvailableScriptableObject(System.Type type)
        {
            return GetAvailableScriptableObjectType(type) != null;
        }

        internal static System.Type GetAvailableScriptableObjectType(System.Type type)
        {
            var editorTypes = GetAvailableScriptableObjectTypes();
            foreach (var editorType in editorTypes)
            {
                var genericTypeArguments = editorType.GetTypeInfo().BaseType.GetTypeInfo().GenericTypeArguments;
                if (genericTypeArguments.Length == 0) continue;
                if (genericTypeArguments[0] == type) return editorType;
            }

            return null;
        }

        internal static List<System.Type> GetCustomEditorTypes()
        {
            return TypeCache.GetTypesDerivedFrom(typeof(GenericEditorWindow<,>)).ToList(); ;
        }

        internal static List<System.Type> GetAvailableScriptableObjectTypes()
        {
            return TypeCache.GetTypesDerivedFrom(typeof(AvailableScriptableObject<>)).ToList(); ;
        }

        internal static List<System.Type> GetSOsTypes()
        {
            return TypeCache.GetTypesDerivedFrom(typeof(GenericScriptableObject)).ToList(); ;
        }

        [OnOpenAsset]
        private static bool OnOpenAsset(int instanceId, int line)
        {
            var temp = EditorUtility.InstanceIDToObject(instanceId) as GenericScriptableObject;
            if (temp != null)
            {
                return OpenScriptableObjectFile(temp);
            }
            return false;
        }

        private static bool OpenScriptableObjectFile(GenericScriptableObject so)
        {
            if (HasCustomEditor(so.GetType()))
            {
                var editorType = GetCustomEditorType(so.GetType());
                var showWindow = editorType.GetMethod("ShowWindow", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (showWindow == null)
                {
                    Debug.LogError(editorType + " has no ShowWindow static function");
                    return false;
                }
                var temp = showWindow.Invoke(null, null);
                var data = System.Convert.ChangeType(temp, editorType);
                {
                    var editorContainer = editorType.GetFieldRecursive("scriptableObject", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).GetValue(data);
                    editorContainer.GetType().GetProperty("Value").SetValue(editorContainer, so);
                }
                editorType.GetMethodRecursive("LoadScriptableObjectInternal", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(data, new object[] { so });
                return true;
            }
            return false;
        }

        public static bool TryGetActiveFolderPath(out string path)
        {
            var _tryGetActiveFolderPath = typeof(ProjectWindowUtil).GetMethod("TryGetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);

            object[] args = new object[] { null };
            bool found = (bool)_tryGetActiveFolderPath.Invoke(null, args);
            path = (string)args[0];

            return found;
        }

        public static void AddStylesheet(this VisualElement root, params string[] styleSheets)
        {
            foreach (var sheet in styleSheets)
            {
                var stylesheet = (StyleSheet)EditorGUIUtility.Load(sheet);

                if (stylesheet == null)
                {
                    Debug.LogError($"Couldnt load stylesheet: '{sheet}'");
                    continue;
                }

                root.styleSheets.Add(stylesheet);
            }
        }

        public static Texture2D RenderStaticPreview(Sprite sprite, int width, int height)
        {
            if (sprite == null) return null;
            var spriteUtilityType = System.Type.GetType("UnityEditor.SpriteUtility,UnityEditor.CoreModule");
            if (spriteUtilityType == null) return null;
            var ret = spriteUtilityType
                        .GetMethod("RenderStaticPreview", new System.Type[] { typeof(Sprite), typeof(Color), typeof(int), typeof(int) })
                        .Invoke(null, new object[] { sprite, Color.white, width, height });
            return ret as Texture2D;
        }

        //https://github.com/lordofduct/spacepuppy-unity-framework/blob/master/SpacepuppyBaseEditor/EditorHelper.cs
        /// <summary>
        /// Gets the object the property represents.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static object GetTargetObjectOfProperty(SerializedProperty prop)
        {
            if (prop == null) return null;

            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            var elements = path.Split('.');
            foreach (var element in elements)
            {
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = GetValue_Imp(obj, elementName, index);
                }
                else
                {
                    obj = GetValue_Imp(obj, element);
                }
            }
            return obj;
        }

        public static object GetTargetObjectOfProperty(SerializedProperty prop, object targetObj)
        {
            var path = prop.propertyPath.Replace(".Array.data[", "[");
            var elements = path.Split('.');
            foreach (var element in elements)
            {
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = System.Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    targetObj = GetValue_Imp(targetObj, elementName, index);
                }
                else
                {
                    targetObj = GetValue_Imp(targetObj, element);
                }
            }
            return targetObj;
        }

        private static object GetValue_Imp(object source, string name)
        {
            if (source == null)
                return null;
            var type = source.GetType();

            while (type != null)
            {
                var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (f != null)
                    return f.GetValue(source);

                var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p != null)
                    return p.GetValue(source, null);

                type = type.BaseType;
            }
            return null;
        }

        private static object GetValue_Imp(object source, string name, int index)
        {
            var enumerable = GetValue_Imp(source, name) as System.Collections.IEnumerable;
            if (enumerable == null) return null;
            var enm = enumerable.GetEnumerator();
            //while (index-- >= 0)
            //    enm.MoveNext();
            //return enm.Current;

            for (int i = 0; i <= index; i++)
            {
                if (!enm.MoveNext()) return null;
            }
            return enm.Current;
        }
    }
}
