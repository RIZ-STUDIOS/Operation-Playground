using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RicTools;
using RicTools.ScriptableObjects;

namespace OperationPlayground.ScriptableObjects
{
    public class RoundScriptableObject : GenericScriptableObject
    {
        public int minEnemies;
        public int maxEnemies;
        public float spawnDelay;
        public RoundEnemyData[] enemies;
    }

    [System.Serializable]
    public class RoundEnemyData
    {
        public int minEnemies;
        public EnemyScriptableObject enemySo;
    }
}