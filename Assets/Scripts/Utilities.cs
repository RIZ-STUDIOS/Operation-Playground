using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public static class Utilities
    {

        public static T GetIfNull<T>(this Behaviour script, ref T component)
        {
            if (component == null)
                component = script.GetComponentInChildren<T>(true);

            return component;
        }

        public static T[] GetIfNull<T>(this Behaviour script, ref T[] component)
        {
            if (component == null)
                component = script.GetComponentsInChildren<T>(true);

            return component;
        }

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();

            if (!component)
                component = gameObject.AddComponent<T>();

            return component;
        }
    }
}
