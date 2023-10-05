using RicTools;
using RicTools.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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