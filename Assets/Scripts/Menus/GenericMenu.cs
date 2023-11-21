using System.Collections;
using UnityEngine;

namespace OperationPlayground.Menus
{
    public class GenericMenu : MonoBehaviour
    {
        public IEnumerator TransitionMenu(CanvasGroup menuCG, bool isEntering, Vector2 direction, float speed = 2)
        {
            RectTransform menuRT = menuCG.GetComponent<RectTransform>();

            Vector2 refRes, newPos, xDirection, yDirection, alphaDirection;

            refRes = new Vector2(Screen.width, Screen.height);
            refRes.x *= direction.x;
            refRes.y *= direction.y;

            #region Assign Vectors
            if (isEntering)
            {
                menuRT.anchoredPosition = refRes;
                menuCG.alpha = 0;

                xDirection = new Vector2(refRes.x, 0);
                yDirection = new Vector2(refRes.y, 0);
                alphaDirection = new Vector2(menuCG.alpha, 1);
            }
            else
            {
                menuCG.alpha = 1;

                xDirection = new Vector2(0, refRes.x);
                yDirection = new Vector2(0, refRes.y);
                alphaDirection = new Vector2(menuCG.alpha, 0);
            }
            #endregion

            #region Lerp Vectors
            float progress = 0;
            while (progress < 1)
            {
                progress += Time.deltaTime * speed;

                newPos.x = Mathf.Lerp(xDirection.x, xDirection.y, progress);
                newPos.y = Mathf.Lerp(yDirection.x, yDirection.y, progress);
                menuCG.alpha = Mathf.Lerp(alphaDirection.x, alphaDirection.y, progress);
                menuRT.anchoredPosition = newPos;

                yield return null;
            }
            #endregion

            #region Assign Absolutes
            if (isEntering)
            {
                menuRT.anchoredPosition = Vector2.zero;
                menuCG.alpha = 1;
                menuCG.blocksRaycasts = true;
                menuCG.interactable = true;
            }
            else
            {
                menuRT.anchoredPosition = refRes;
                menuCG.alpha = 0;
                menuCG.blocksRaycasts = false;
                menuCG.interactable = false;
            }
            #endregion
        }
    }
}
