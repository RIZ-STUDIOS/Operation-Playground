using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RicTools.Attributes
{
    public class RenameAttribute : PropertyAttribute
    {
        public readonly string Name;

        public RenameAttribute(string name)
        {
            Name = name;
        }
    }
}
