using RicTools;
using RicTools.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.ScriptableObjects
{
    public class BuildingScriptableObject : GenericScriptableObject
    {
        public GameObject visual;
        public GameObject prefab;
        public Vector3 timerOffset;
        public Vector3 placementOffset;
        public Vector3 boundsToCheck;
        public int resourceCost;
        public Sprite buildingSprite;
        public int health;
    }
}