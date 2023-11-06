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
                component = script.GetComponentInChildren<T>();

            return component;
        }

        public static T[] GetIfNull<T>(this Behaviour script, ref T[] component)
        {
            if (component == null)
                component = script.GetComponentsInChildren<T>();

            return component;
        }
    }
}
