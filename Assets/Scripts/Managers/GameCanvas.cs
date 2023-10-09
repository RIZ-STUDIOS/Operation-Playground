using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Managers
{
    public class GameCanvas : MonoBehaviour
    {
        [NonSerialized]
        public Canvas canvas;

        [NonSerialized]
        public CanvasGroup canvasGroup;

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>();
            GameManager.Instance.gameCanvas = canvas;
        }

        private void Start()
        {
            GameManager.Instance.lobbyMenu.onLobbyFinished += () =>
            {
                canvasGroup.alpha = 1;
                canvasGroup.blocksRaycasts = true;
            };
        }
    }
}
