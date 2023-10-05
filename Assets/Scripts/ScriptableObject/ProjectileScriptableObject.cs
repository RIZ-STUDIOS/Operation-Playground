using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RicTools;
using RicTools.ScriptableObjects;

namespace OperationPlayground.ScriptableObjects
{
    public class ProjectileScriptableObject : GenericScriptableObject
    {
        public GameObject prefab;
        public float speed;
        public float travelDuration;
    }
}