using OperationPlayground.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public class GameLevelData : MonoBehaviour
    {
        public Transform[] spawnLocations;
        public Transform mapCamera;

        private void Awake()
        {
            GameManager.Instance.gameLevelData = this;
        }
    }
}
