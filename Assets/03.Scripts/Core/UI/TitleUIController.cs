using UnityEngine;
using UnityEngine.UI;

public class TitleUIController : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject optionPanel;

    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.SetupSliders(masterSlider, bgmSlider, sfxSlider);
        }
    }

    public void OnClickNewGame()
    {
        UIManager.Instance?.OnClickNewGame();
    }

    public void OnClickLoad()
    {
        UIManager.Instance?.OnClickLoad();
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
        UIManager.Instance?.OnClickExit();
    }
}