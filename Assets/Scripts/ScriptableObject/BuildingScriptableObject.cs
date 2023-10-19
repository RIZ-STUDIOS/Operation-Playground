using RicTools;
using RicTools.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.ScriptableObjects
{
    public enum BuildingType
    {
        Generic,
        Turret
    }

    public class BuildingScriptableObject : GenericScriptableObject
    {
        public GameObject prefab;
        public float health;
        public float placementDistance;
        public BuildingType buildingType;
        public Vector3 boundsToCheck;
    }
}