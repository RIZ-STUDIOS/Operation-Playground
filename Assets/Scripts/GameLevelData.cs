using OperationPlayground.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public class GameLevelData : MonoBehaviour
    {
        public Transform[] spawnLocations;

        private void Awake()
        {
            GameManager.Instance.gameLevelData = this;
        }
    }
}
