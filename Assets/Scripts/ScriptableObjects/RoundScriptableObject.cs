using RicTools;
using RicTools.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.ScriptableObjects
{
    public class RoundScriptableObject : GenericScriptableObject
    {
        public int minEnemies;
        public int maxEnemies;
        public float spawnDelay;
        public int supplyReward;
        public RoundEnemyData[] enemies;
    }

    [System.Serializable]
    public class RoundEnemyData
    {
        [System.NonSerialized]
        public int spawned;
        public EnemyScriptableObject enemySo;
    }
}