using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx : MonoSingleton<SceneManagerEx>
{
    [SerializeField] private string InGameSceneName = "InGameScene";

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
        
        //Debug.Log($"[SceneManagerEx] 씬 전환 완료 된 건가? : {sceneName}");
        //if(sceneName == InGameSceneName)
        //{
        //    Debug.Log($"[SceneManagerEx] 메인 씬 로드 완료!");
        //    SaveManager.Instance.LoadGameData();
        //    Debug.Log($"[SceneManagerEx] 메인 씬 세이브 데이터도 로드 완료!");
        //}
    }

    public void RestartCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        LoadScene(currentSceneName);
    }
}