using OperationPlayground.Interactables;
using OperationPlayground.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OperationPlayground.Player
{
    public class PlayerCanvas : MonoBehaviour
    {
        public CanvasGroup interactCG;

        private PlayerManager pM;

        private Coroutine fadeCoroutine;

        private void Awake()
        {
            pM = GetComponentInParent<PlayerManager>();
            pM.PlayerInteraction.onSetInteractable += OnSetInteractable;
        }

        private void OnSetInteractable(Interactable interactable)
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);

            if (interactable)
                fadeCoroutine = StartCoroutine(FadeCanvasElement(interactCG, true));
            else 
                fadeCoroutine = StartCoroutine(FadeCanvasElement(interactCG, false));
        }

        public IEnumerator FadeCanvasElement(CanvasGroup cg, bool isFadeIn, float fadeSpeed = 2)
        {
            Vector2 fadeVector;
            if (isFadeIn) fadeVector = new Vector2(cg.alpha, 1);
            else fadeVector = new Vector2(cg.alpha, 0);

            float progress = 0;
            while (progress < 1)
            {
                progress += Time.deltaTime * fadeSpeed;
                cg.alpha = Mathf.Lerp(fadeVector.x, fadeVector.y, progress);

                yield return null;
            }
            cg.alpha = fadeVector.y;

            fadeCoroutine = null;
        }
    }
}
