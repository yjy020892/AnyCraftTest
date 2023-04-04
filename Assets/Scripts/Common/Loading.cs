using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace JY
{
    public class Loading : MonoBehaviour
    {
        static string nextScene;

        bool isAsync = false;

        public static void LoadScene(string sceneName)
        {
            nextScene = sceneName;
            SceneManager.LoadScene("Loading");
        }

        private void Start()
        {
            StartCoroutine(LoadSceneAsync(nextScene));
        }

        /// <summary>
        /// 비동기 씬 로드
        /// 미리 어드레서블 로드
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public IEnumerator LoadSceneAsync(string sceneName)
        {
            Debug.Log($"sceneName : {sceneName}");
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

            asyncOperation.allowSceneActivation = false;

            while (!asyncOperation.isDone)
            {
                if (asyncOperation.progress >= 0.9f)
                {
                    StartCoroutine(AddressableManager.GetInstance.AddressablesAsync("ModelUI", (x) => {
                        isAsync = x;
                    }));

                    yield return new WaitUntil(() => isAsync);

                    if (AddressableManager.GetInstance.addressablesItemObj != null)
                        asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }
}