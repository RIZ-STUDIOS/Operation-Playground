using System.Collections;
using UnityEngine;

namespace OperationPlayground.Menus
{
    public class GenericMenu : MonoBehaviour
    {
        public IEnumerator TransitionMenu(CanvasGroup menu, bool isEntering, Vector2 direction, float speed = 2)
        {
            if (direction.x > 0) direction.x = Mathf.Ceil(direction.x);
            else direction.x = Mathf.Floor(direction.x);

            if (direction.y > 0) direction.y = Mathf.Ceil(direction.y);
            else direction.y = Mathf.Floor(direction.y);

            RectTransform menuRT = menu.GetComponent<RectTransform>();

            Vector2 refRes = new Vector2(Screen.width, Screen.height);

            refRes.x *= Vector3.Normalize(direction).x;
            refRes.y *= Vector3.Normalize(direction).y;

            Vector2 newPos;

            if (isEntering)
            {
                menuRT.anchoredPosition = refRes;

                menu.alpha = 0;
                float progress = 0;


                while (menu.alpha < 1)
                {
                    progress += Time.deltaTime * speed;

                    menu.alpha = progress;

                    newPos.x = Mathf.Lerp(refRes.x, 0, progress);
                    newPos.y = Mathf.Lerp(refRes.y, 0, progress);
                    menuRT.anchoredPosition = newPos;

                    yield return null;
                }

                menuRT.anchoredPosition = Vector2.zero;
                menu.alpha = 1;
                menu.blocksRaycasts = true;
                menu.interactable = true;
            }
            else
            {
                menu.alpha = 1;
                float progress = 0;

                while (menu.alpha > 0)
                {
                    progress += Time.deltaTime * speed;

                    menu.alpha = 1 - progress;

                    newPos.x = Mathf.Lerp(0, refRes.x, progress);
                    newPos.y = Mathf.Lerp(0, refRes.y, progress);
                    menuRT.anchoredPosition = newPos;

                    yield return null;
                }

                menuRT.anchoredPosition = refRes;
                menu.alpha = 0;
                menu.blocksRaycasts = false;
                menu.interactable = false;
            }
        }
    }
}
