using RicTools.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public class GameManager : SingletonGenericManager<GameManager>
    {
        public Canvas gameCanvas;
        public Camera gameCamera;
    }
}
