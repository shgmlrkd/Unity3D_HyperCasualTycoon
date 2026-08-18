using UnityEngine;

public class UI_PauseMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject optionPanel;

    [Header("Confirm Modal Popup")]
    [SerializeField] private UI_ConfirmModal confirmModal;

    [Header("Scene Settings")]
    [SerializeField] private string titleSceneName = "TitleScene";

    private void Start()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.Subscribe(EventType.OnGameStateChanged, OnGameStateChanged);
        }
    }

    private void OnDestroy()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.Unsubscribe(EventType.OnGameStateChanged, OnGameStateChanged);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionPanel != null && optionPanel.activeSelf)
            {
                OnClickCloseOption();
                return;
            }

            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.CurrentState == GameState.Play)
                {
                    GameManager.Instance.PauseGame();
                }
                else if (GameManager.Instance.CurrentState == GameState.Pause)
                {
                    GameManager.Instance.ResumeGame();
                }
            }
        }
    }

    private void OnGameStateChanged(object param)
    {
        if (GameManager.Instance == null || pausePanel == null) return;

        bool isPaused = (GameManager.Instance.CurrentState == GameState.Pause);
        pausePanel.SetActive(isPaused);
    }

    public void OnClickResume()
    {
        GameManager.Instance?.ResumeGame();
    }

    public void OnClickSave()
    {
        if (SaveManager.Instance == null) return;

        if (SaveManager.Instance.HasSaveFile())
        {
            confirmModal?.ShowConfirm(
                "You already have a save file.\nDo you want to overwrite?",
                onYes: ExecuteSave
            );
        }
        else
        {
            ExecuteSave();
        }
    }

    private void ExecuteSave()
    {
        bool success = SaveManager.Instance.SaveGameData();

        if (success)
        {
            confirmModal?.ShowAlert("Game Save Complete.");
        }
        else
        {
            confirmModal?.ShowAlert("Game Save Failed. Try Again Later.");
        }
    }

    public void OnClickToTitle()
    {
        if (SaveManager.Instance != null && SaveManager.Instance.IsDirty)
        {
            confirmModal?.ShowConfirm(
                "You have not saved the game.\nDo you want to go to title without saving?",
                onYes: GoToTitleScene
            );
        }
        else
        {
            GoToTitleScene();
        }
    }

    private void GoToTitleScene()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResumeGame();
        }

        Time.timeScale = 1f;
        SceneManagerEx.Instance?.LoadScene(titleSceneName);
    }

    public void OnClickOpenOption()
    {
        if (optionPanel != null)
        {
            optionPanel.SetActive(true);
        }
    }

    public void OnClickCloseOption()
    {
        if (optionPanel != null)
        {
            optionPanel.SetActive(false);
        }
    }
}