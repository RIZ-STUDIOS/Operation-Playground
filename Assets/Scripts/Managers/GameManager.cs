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
        public Camera gameCamera;
        public TowerHealth towerHealth;
        public LoseWinUI loseWinUI;
    }
}
