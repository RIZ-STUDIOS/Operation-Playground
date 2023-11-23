using OperationPlayground.Player.DefendPoint;
using OperationPlayground.Rounds;
using RicTools.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public class GameStateManager : GenericManager<GameStateManager>
    {
        public RoundManager roundManager;
        public DefendPointData defendPointData;

        protected override void Awake()
        {
            base.Awake();
            roundManager = RoundManager.Instance;
            roundManager.onAllRoundsFinish += OnGameWon;

            defendPointData.DefendPointHealth.onDeath += OnGameLost;
        }

        private void OnGameWon()
        {

        }

        private void OnGameLost()
        {

        }
    }
}
