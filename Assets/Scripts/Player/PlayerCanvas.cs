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
        public CanvasGroup supplyShopCG;

        public PlayerManager pM;

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
                fadeCoroutine = StartCoroutine(ToggleCanvasElement(interactCG, true));
            else 
                fadeCoroutine = StartCoroutine(ToggleCanvasElement(interactCG, false));
        }

        public IEnumerator ToggleCanvasElement(CanvasGroup cG, bool isFadeIn, bool interactable = false, float fadeSpeed = 2)
        {
            Vector2 fadeVector;

            if (isFadeIn) fadeVector = new Vector2(cG.alpha, 1);
            else fadeVector = new Vector2(cG.alpha, 0);

            float progress = 0;
            while (progress < 1)
            {
                progress += Time.deltaTime * fadeSpeed;
                cG.alpha = Mathf.Lerp(fadeVector.x, fadeVector.y, progress);

                yield return null;
            }
            cG.alpha = fadeVector.y;

            if (interactable)
            {
                if (isFadeIn) cG.interactable = true;
                else cG.interactable = false;
            }

            fadeCoroutine = null;
        }
    }
}
