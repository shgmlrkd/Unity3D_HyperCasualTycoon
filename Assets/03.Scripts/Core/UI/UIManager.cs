using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [Header("Scene Names")]
    [SerializeField] private string titleSceneName = "TitleScene";
    [SerializeField] private string inGameSceneName = "InGameScene";

    private bool isNewGame = false;
    public bool IsNewGame => isNewGame;

    public void OnClickNewGame()
    {
        Debug.Log("[UIManager] New Game 버튼 클릭 - 매니저 데이터 리셋 후 이동");

        if (SaveManager.Instance != null)
        {
            Debug.Log("세이브 매니저, 뉴 게임 준비.");
            SaveManager.Instance.PrepareNewGame();
        }
        
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.ResetData();
        }

        if (SceneManagerEx.Instance != null)
        {
            SetNewGame(true);
            SceneManagerEx.Instance.LoadScene(inGameSceneName);
        }
    }

    public void OnClickLoad()
    {
        Debug.Log("[UIManager] Load Game 버튼 클릭 - 세이브 파일 로드 후 진입");

        if (SaveManager.Instance == null)
        {
            Debug.Log("[UIManager] SaveManager가 없음.");
            return;

            //Debug.Log("여기서 데이터 불러오기 해야됨.");
            //SaveManager.Instance.LoadGameData();
        }

        bool loadSucceeded = SaveManager.Instance.LoadGameData();

        if(!loadSucceeded)
        {
            Debug.LogWarning("[UIManager] 세이브가 제대로 로드되지 않아 인게임 진입 취소합니다.");
            return;
        }

        if(CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.ApplySaveData(SaveManager.Instance.CurrentData);
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

    public void SetNewGame(bool isNewGame)
    {
        this.isNewGame = isNewGame;
    }

    public void OnClickTestSFX()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFXTest();
        }
    }
}