using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Loading : MonoBehaviour
{
    /// <summary>
    /// 비동기 씬 로드
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public static IEnumerator LoadScene(string sceneName)
    {
        Debug.Log($"sceneName : {sceneName}");
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}