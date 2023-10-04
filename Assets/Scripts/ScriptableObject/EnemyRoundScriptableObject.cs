using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RicTools;
using RicTools.ScriptableObjects;
using System;

namespace OperationPlayground.ScriptableObjects
{
    public class EnemyRoundScriptableObject : GenericScriptableObject
    {
        public EnemyRoundData[] enemies;
        public EnemyRoundData[] supportEnemies;
    }

    [Serializable]
    public class EnemyRoundData
    {
        public EnemyScriptableObject enemy;
        public int count;
    }
}