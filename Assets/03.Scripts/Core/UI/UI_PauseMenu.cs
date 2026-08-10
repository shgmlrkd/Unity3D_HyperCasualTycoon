using UnityEngine;
using UnityEngine.UI;

public class UI_PauseMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject optionPanel;

    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private string titleSceneName = "TitleScene";

    private void Start()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.Subscribe(EventManager.EventType.OnGameStateChanged, OnGameStateChanged);
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.SetupSliders(masterSlider, bgmSlider, sfxSlider);
        }
    }

    private void OnDestroy()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.Unsubscribe(EventManager.EventType.OnGameStateChanged, OnGameStateChanged);
        }
    }

    private void OnGameStateChanged(object param)
    {
        if (GameManager.Instance == null || pausePanel == null) return;

        bool isPaused = (GameManager.Instance.CurrentState == GameManager.GameState.Pause);
        pausePanel.SetActive(isPaused);
    }

    public void OnClickResume()
    {
        GameManager.Instance?.ResumeGame();
    }

    public void OnClickSave()
    {
        SaveManager.Instance?.SaveGameData();
    }

    public void OnClickToTitle()
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
        if (optionPanel != null) optionPanel.SetActive(true);
    }

    public void OnClickCloseOption()
    {
        if (optionPanel != null) optionPanel.SetActive(false);
    }
}