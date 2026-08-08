using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [Header("Panels")]
    [SerializeField] private GameObject titlePanel;
    [SerializeField] private GameObject optionPanel;

    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Scene Names")]
    [SerializeField] private string titleSceneName = "TitleScene";
    [SerializeField] private string inGameSceneName = "InGameScene";
   
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        InitUI();
    }

    public void InitUI()
    {
        OptionData option = SaveManager.Instance != null ? SaveManager.Instance.LoadOptionData() : new OptionData();

        if (masterSlider != null) masterSlider.value = option.masterVol;
        if (bgmSlider != null) bgmSlider.value = option.bgmVol;
        if (sfxSlider != null) sfxSlider.value = option.sfxVol;

        OnMasterVolumeChanged(option.masterVol);
        OnBGMVolumeChanged(option.bgmVol);
        OnSFXVolumeChanged(option.sfxVol);

        if (masterSlider != null)
        {
            masterSlider.onValueChanged.RemoveAllListeners();
            masterSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        }
        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.RemoveAllListeners();
            bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        }
        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.RemoveAllListeners();
            sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }
    }
    //private void Start()
    //{
    //    OptionData option = SaveManager.Instance != null ? SaveManager.Instance.LoadOptionData() : new OptionData();

    //    if (masterSlider != null) masterSlider.value = option.masterVol;
    //    if (bgmSlider != null) bgmSlider.value = option.bgmVol;
    //    if (sfxSlider != null) sfxSlider.value = option.sfxVol;

    //    OnMasterVolumeChanged(option.masterVol);
    //    OnBGMVolumeChanged(option.bgmVol);
    //    OnSFXVolumeChanged(option.sfxVol);

    //    if (masterSlider != null) masterSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
    //    if (bgmSlider != null) bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
    //    if (sfxSlider != null) sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    //}

    public void OnClickNewGame()
    {
        Debug.Log("[UIManager] New Game Button Clicked.");

        if (SceneManagerEx.Instance != null)
        {
            SceneManagerEx.Instance.LoadScene(inGameSceneName);
        }
    }

    public void OnClickLoad()
    {
        Debug.Log("[UIManager] Load Game Button Clicked.");

        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.LoadGameData();
        }

        if (SceneManagerEx.Instance != null)
        {
            SceneManagerEx.Instance.LoadScene(inGameSceneName);
        }
    }

    public void OnClickOpenOption()
    {
        if (optionPanel != null) optionPanel.SetActive(true);
    }

    public void OnClickCloseOption()
    {
        if (optionPanel != null) optionPanel.SetActive(false);
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

    public void OnMasterVolumeChanged(float value)
    {
        AudioListener.volume = value;
        SaveOption();
    }

    public void OnBGMVolumeChanged(float value)
    {
        if (SoundManager.Instance != null) SoundManager.Instance.SetBGMVolume(value);
        SaveOption();
    }

    public void OnSFXVolumeChanged(float value)
    {
        if (SoundManager.Instance != null) SoundManager.Instance.SetSFXVolume(value);
        SaveOption();
    }

    private void SaveOption()
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.SaveOptionData(
                masterSlider != null ? masterSlider.value : 1f,
                bgmSlider != null ? bgmSlider.value : 1f,
                sfxSlider != null ? sfxSlider.value : 1f
            );
        }
    }
}