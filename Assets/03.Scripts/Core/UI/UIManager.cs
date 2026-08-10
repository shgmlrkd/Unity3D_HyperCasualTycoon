using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [Header("Scene Names")]
    [SerializeField] private string titleSceneName = "TitleScene";
    [SerializeField] private string inGameSceneName = "InGameScene";

    public void OnClickNewGame()
    {
        Debug.Log("[UIManager] New Game 버튼 클릭 - 매니저 데이터 리셋 후 이동");

        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.ResetData();
        }

        if (SceneManagerEx.Instance != null)
        {
            SceneManagerEx.Instance.LoadScene(inGameSceneName);
        }
    }

    public void OnClickLoad()
    {
        Debug.Log("[UIManager] Load Game 버튼 클릭.");

        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.LoadGameData();
        }

        if (SceneManagerEx.Instance != null)
        {
            SceneManagerEx.Instance.LoadScene(inGameSceneName);
        }
    }

    public void OnClickExit()
    {
        Debug.Log("[UIManager] 게임 종료.");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnClickTestSFX()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFXTest();
        }
    }
}