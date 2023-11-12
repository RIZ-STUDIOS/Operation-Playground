using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Menus
{
    public class MainSubMenu : MonoBehaviour
    {
        public CanvasGroup canvasGroup;
        public Vector2 entryDirection;
        //public float transitionSpeed;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }
}
