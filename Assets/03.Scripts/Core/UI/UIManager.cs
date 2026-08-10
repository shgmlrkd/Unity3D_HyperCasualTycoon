using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [Header("Scene Names")]
    [SerializeField] private string titleSceneName = "TitleScene";
    [SerializeField] private string inGameSceneName = "InGameScene";

    private Slider currentMasterSlider;
    private Slider currentBGMSlider;
    private Slider currentSFXSlider;

    public void SetupSliders(Slider master, Slider bgm, Slider sfx)
    {
        currentMasterSlider = master;
        currentBGMSlider = bgm;
        currentSFXSlider = sfx;

        OptionData option = SaveManager.Instance != null ? SaveManager.Instance.LoadOptionData() : new OptionData();

        if (currentMasterSlider != null)
        {
            currentMasterSlider.onValueChanged.RemoveAllListeners();
            currentMasterSlider.value = option.masterVol;
            currentMasterSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        }

        if (currentBGMSlider != null)
        {
            currentBGMSlider.onValueChanged.RemoveAllListeners();
            currentBGMSlider.value = option.bgmVol;
            currentBGMSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        }

        if (currentSFXSlider != null)
        {
            currentSFXSlider.onValueChanged.RemoveAllListeners();
            currentSFXSlider.value = option.sfxVol;
            currentSFXSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }

        OnMasterVolumeChanged(option.masterVol);
        OnBGMVolumeChanged(option.bgmVol);
        OnSFXVolumeChanged(option.sfxVol);
    }

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

    public void OnMasterVolumeChanged(float value)
    {
        AudioListener.volume = value;
        SaveOption();
    }

    public void OnBGMVolumeChanged(float value)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetBGMVolume(value);
        }
        SaveOption();
    }

    public void OnSFXVolumeChanged(float value)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetSFXVolume(value);
        }
        SaveOption();
    }

    public void OnClickTestSFX()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFXTest();
        }
    }

    private void SaveOption()
    {
        if (SaveManager.Instance != null)
        {
            float master = currentMasterSlider != null ? currentMasterSlider.value : 1f;
            float bgm = currentBGMSlider != null ? currentBGMSlider.value : 1f;
            float sfx = currentSFXSlider != null ? currentSFXSlider.value : 1f;

            SaveManager.Instance.SaveOptionData(master, bgm, sfx);
        }
    }
}