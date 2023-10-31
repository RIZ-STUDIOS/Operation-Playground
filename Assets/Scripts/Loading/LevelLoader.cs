using RicTools.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace OperationPlayground.Loading
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField]
        private Slider progressBar;

        [SerializeField]
        private TextMeshProUGUI errorText;

        [SerializeField]
        private CanvasGroup fadeCanvasGroup;    

        [SerializeField]
        private float fakeDurationLength = 0.15f;

        private static int sceneToLoad;
        private static System.Action onLoad;

        static LevelLoader()
        {
            sceneToLoad = -1;
        }

        private void Awake()
        {
            errorText.text = "";
            if (sceneToLoad < 0)
            {
                errorText.text = "Error finding scene to load!";
                return;
            }
            StartCoroutine(LoadSceneCoroutine());
        }

        public static void LoadScene(int sceneId, System.Action onLoad = null)
        {
            Time.timeScale = 1;
            sceneToLoad = sceneId;
            LevelLoader.onLoad = onLoad;
            SceneManager.LoadScene("LoadingScene");
        }

        public static void LoadScene(string sceneName, System.Action onLoad = null)
        {
            LoadScene(SceneUtility.GetBuildIndexByScenePath(sceneName), onLoad);
        }

        private IEnumerator LoadSceneCoroutine()
        {
            progressBar.value = 0;
            yield return RicUtilities.FadeInCanvasGroup(fadeCanvasGroup, 0.1f);
            yield return new WaitForSeconds(fakeDurationLength);
            AsyncOperation newScene = SceneManager.LoadSceneAsync(sceneToLoad);
            newScene.completed += (_) =>
            {
                sceneToLoad = -1;
                onLoad?.Invoke();
                onLoad = null;
            };
            while (!newScene.isDone)
            {
                progressBar.value = Mathf.Clamp01(newScene.progress / 0.9f);
                if (progressBar.value >= 0.9f)
                {
                    progressBar.value = 1;
                    break;
                }
                yield return null;
            }
        }
    }
}
