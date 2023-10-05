using RicTools;
using RicTools.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.ScriptableObjects
{
    public class ProjectileScriptableObject : GenericScriptableObject
    {
        public GameObject prefab;
        public float speed;
        public float travelDuration;
    }
}