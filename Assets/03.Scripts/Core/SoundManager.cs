using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    [Header("오디오 소스 설정")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        InitVolume();
    }

    public void InitVolume()
    {
        OptionData option = SaveManager.Instance != null ? SaveManager.Instance.LoadOptionData() : new OptionData();

        AudioListener.volume = option.masterVol;
        SetBGMVolume(option.bgmVol);
        SetSFXVolume(option.sfxVol);
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null || bgmSource == null) return;

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;

        sfxSource.PlayOneShot(clip);
    }

    public void PlaySFXTest()
    {
        if (sfxSource == null || sfxSource.clip == null) return;
        PlaySFX(sfxSource.clip);
    }

    public void SetBGMVolume(float volume)
    {
        if (bgmSource != null)
        {
            bgmSource.volume = volume;
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = volume;
        }
    }
}