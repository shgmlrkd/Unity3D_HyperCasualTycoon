using UnityEngine;
using UnityEngine.UI;

public class UI_OptionPanel : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button testSFXButton;

    private void OnEnable()
    {
        InitSliders();
    }

    private void InitSliders()
    {
        OptionData option = SaveManager.Instance != null ? SaveManager.Instance.LoadOptionData() : new OptionData();

        if (masterSlider != null) masterSlider.value = option.masterVol;
        if (bgmSlider != null) bgmSlider.value = option.bgmVol;
        if (sfxSlider != null) sfxSlider.value = option.sfxVol;
        if (testSFXButton != null)
        {
            testSFXButton.onClick.RemoveAllListeners();
            testSFXButton.onClick.AddListener(() =>
            {
                SoundManager.Instance?.PlaySFXTest();
            });
        }

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

    private void OnMasterVolumeChanged(float value)
    {
        AudioListener.volume = value;
        SaveCurrentOption();
    }

    private void OnBGMVolumeChanged(float value)
    {
        SoundManager.Instance?.SetBGMVolume(value);
        SaveCurrentOption();
    }

    private void OnSFXVolumeChanged(float value)
    {
        SoundManager.Instance?.SetSFXVolume(value);
        SaveCurrentOption();
    }

    private void SaveCurrentOption()
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

    public void OnClickClose()
    {
        SoundManager.Instance.PlaySFX(SoundType.ButtonClick);
        gameObject.SetActive(false);
    }
}