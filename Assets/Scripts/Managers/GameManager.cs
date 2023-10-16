using OperationPlayground.Enemy;
using OperationPlayground.SupplyCreates;
using OperationPlayground.UI;
using RicTools.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Managers
{
    public class GameManager : SingletonGenericManager<GameManager>
    {
        public Canvas gameCanvas;
        public GameCamera gameCamera;
        public TowerHealth towerHealth;
        public LoseWinUI loseWinUI;
        public LobbyMenu lobbyMenu;
        public SupplyCrateManager supplyCrateManager;
        public EnemyRoundManager enemyRoundManager;
    }
}
