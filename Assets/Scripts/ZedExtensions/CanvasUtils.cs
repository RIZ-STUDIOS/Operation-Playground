using Codice.CM.Common;
using OperationPlayground.Interactables;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OperationPlayground.ZedExtensions
{
    public static class CanvasUtils
    {
        public static IEnumerator FadeIn(this CanvasGroup canvasGroup, bool interactable = false, bool blocksRaycasts = false, float fadeSpeedMod = 2)
        {
            IEnumerator lerpIn = LerpCanvasAlphaOverTime(canvasGroup, 1);
            yield return lerpIn;

            if (interactable) canvasGroup.interactable = true;
            if (blocksRaycasts) canvasGroup.blocksRaycasts = true;
        }

        public static IEnumerator FadeOut(this CanvasGroup canvasGroup, bool interactable = false, bool blocksRaycasts = false, float fadeSpeedMod = 2)
        {
            if (interactable) canvasGroup.interactable = false;
            if (blocksRaycasts) canvasGroup.blocksRaycasts = false;

            IEnumerator lerpOut = LerpCanvasAlphaOverTime(canvasGroup, 0);
            yield return lerpOut;
        }

        public static IEnumerator FadeInThenOut(this CanvasGroup canvasGroup, bool interactable = false, bool blocksRaycasts = false, float fadeSpeedMod = 2, float duration = 0.5f)
        {
            IEnumerator lerpIn = LerpCanvasAlphaOverTime(canvasGroup, 1);
            yield return lerpIn;

            if (interactable) canvasGroup.interactable = true;
            if (blocksRaycasts) canvasGroup.blocksRaycasts = true;

            yield return new WaitForSeconds(duration);

            if (interactable) canvasGroup.interactable = false;
            if (blocksRaycasts) canvasGroup.blocksRaycasts = false;

            IEnumerator lerpOut = LerpCanvasAlphaOverTime(canvasGroup, 0);
            yield return lerpOut;
        }

        private static IEnumerator LerpCanvasAlphaOverTime(CanvasGroup canvasGroup, float targetValue, float lerpSpeedMod = 3)
        {
            Vector2 lerpVector = new Vector2(canvasGroup.alpha, targetValue);

            float progress = 0;
            while (progress < 1)
            {
                progress += Time.deltaTime * lerpSpeedMod;

                canvasGroup.alpha = Mathf.Lerp(lerpVector.x, lerpVector.y, progress);

                yield return null;
            }

            yield return lerpVector.y;
        }
    }
}
