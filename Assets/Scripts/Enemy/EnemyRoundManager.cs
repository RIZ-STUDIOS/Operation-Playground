using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RicTools.Managers;
using System;

namespace OperationPlayground
{
    public class EnemyRoundManager : GenericManager<EnemyRoundManager>
    {
        protected override bool DestroyIfFound => false;

        [SerializeField]
        private EnemyRoundScriptableObject[] rounds;

        public void StartRound()
        {

        }
    }
}
