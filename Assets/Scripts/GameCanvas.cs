using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public class GameCanvas : MonoBehaviour
    {
        [NonSerialized]
        public Canvas canvas;

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            GameManager.Instance.gameCanvas = canvas;
        }
    }
}
