using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx : MonoSingleton<SceneManagerEx>
{
    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("[SceneManagerEx] 씬 이름을 제대로 넣어야죠! 으이구!");
            return;
        }

        Time.timeScale = 1f;

        Debug.Log($"[SceneManagerEx] 씬 전환 시작: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    public void RestartCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        LoadScene(currentSceneName);
    }
}