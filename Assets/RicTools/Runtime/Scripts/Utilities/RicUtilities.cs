using RicTools.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace RicTools.Utilities
{
    public static class RicUtilities
    {
        public static T GetAvailableScriptableObject<T, D>() where T : AvailableScriptableObject<D> where D : GenericScriptableObject
        {
            var so = Resources.Load<T>(PathConstants.RESOURCES_AVAILABLES_FOLDER + "/" + GetAvailableScriptableObjectName(typeof(T)));

#if UNITY_EDITOR
            if (!so)
            {
                so = ScriptableObject.CreateInstance<T>();
                CreateAssetFolder(PathConstants.RESOURCES_AVAILABLES_FOLDER);
                AssetDatabase.CreateAsset(so, PathConstants.RESOURCES_AVAILABLES_FOLDER + "/" + GetAvailableScriptableObjectName(typeof(T)) + ".asset");
                AssetDatabase.SaveAssets();
            }
#endif

            return so;
        }

        public static string GetAvailableScriptableObjectName(System.Type type)
        {
            var name = type.Name;
            name = name.Replace("ScriptableObject", "");
            return name;
        }

        public static string GetAvailableScriptableObjectPath(System.Type type)
        {
            return $"{PathConstants.AVAILABLES_FOLDER}/{GetAvailableScriptableObjectName(type)}.asset";
        }

        public static string GetScriptableObjectPath(System.Type type)
        {
            var name = type.Name;
            name = name.Replace("ScriptableObject", "");
            return $"{PathConstants.ASSETS_FOLDER}/{PathConstants.SCRIPTABLES_FOLDER}/{name}s";
        }

        public static string ToFriendlyCase(this string PascalString)
        {
            return Regex.Replace(PascalString, "(?!^)([A-Z])", " $1");
        }

        // https://stackoverflow.com/questions/457676/check-if-a-class-is-derived-from-a-generic-class
        public static bool IsSubclassOfRawGeneric(System.Type generic, System.Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

#if UNITY_EDITOR
        public static void CreateAssetFolder(string folderPath)
        {
            if (Path.HasExtension(folderPath))
                folderPath = Path.GetDirectoryName(folderPath);
            folderPath = folderPath.Replace("\\", "/");
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                string[] dirs = folderPath.Split('/');
                string path = dirs[0];
                for (int i = 1; i < dirs.Length; i++)
                {
                    if (!AssetDatabase.IsValidFolder(path + $"/{dirs[i]}"))
                    {
                        AssetDatabase.CreateFolder(path, dirs[i]);
                    }
                    path += $"/{dirs[i]}";
                }
            }
        }
#endif
        public static T GetRandomElement<T>(this IEnumerable<T> array)
        {
            return array.ElementAt(Random.Range(0, array.Count()));
        }

        public static T GetRandomElement<T>(this System.Array array)
        {
            return (T)array.GetValue(Random.Range(0, array.Length));
        }
    }
}
