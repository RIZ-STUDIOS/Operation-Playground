using OperationPlayground.Interactables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.ZedExtensions
{
    public static class CanvasUtils
    {
        public static IEnumerator FadeIn(this CanvasGroup canvasGroup, bool interactable = false, bool blocksRaycasts = false, float fadeSpeedMod = 2)
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime * fadeSpeedMod;
                yield return null;
            }

            canvasGroup.alpha = 1;

            if (interactable) canvasGroup.interactable = true;
            if (blocksRaycasts) canvasGroup.blocksRaycasts = true;
        }

        public static IEnumerator FadeOut(this CanvasGroup canvasGroup, bool interactable = false, bool blocksRaycasts = false, float fadeSpeedMod = 2)
        {
            if (interactable) canvasGroup.interactable = false;
            if (blocksRaycasts) canvasGroup.blocksRaycasts = false;

            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime * fadeSpeedMod;
                yield return null;
            }

            canvasGroup.alpha = 0;
        }

        public static IEnumerator FadeInThenOut(this CanvasGroup canvasGroup, bool interactable = false, bool blocksRaycasts = false, float fadeSpeedMod = 2, float duration = 0.5f)
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime * fadeSpeedMod;
                yield return null;
            }

            if (interactable) canvasGroup.interactable = true;
            if (blocksRaycasts) canvasGroup.blocksRaycasts = true;

            yield return new WaitForSeconds(duration);

            if (interactable) canvasGroup.interactable = false;
            if (blocksRaycasts) canvasGroup.blocksRaycasts = false;

            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime * fadeSpeedMod;
                yield return null;
            }

            canvasGroup.alpha = 0;
        }
    }
}
