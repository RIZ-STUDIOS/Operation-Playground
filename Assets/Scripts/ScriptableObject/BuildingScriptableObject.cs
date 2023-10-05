using RicTools;
using RicTools.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.ScriptableObjects
{
    public class BuildingScriptableObject : GenericScriptableObject
    {
        public GameObject prefab;
        public int health;
        public int placementDistance;
        public Vector3 boundsToCheck;
    }
}