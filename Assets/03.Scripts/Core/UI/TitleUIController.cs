using UnityEngine;

public class TitleUIController : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject optionPanel;

    public void OnClickNewGame()
    {
        SoundManager.Instance.PlaySFX(SoundType.ButtonClick);
        UIManager.Instance?.OnClickNewGame();
    }

    public void OnClickLoad()
    {
        SoundManager.Instance.PlaySFX(SoundType.ButtonClick);
        UIManager.Instance?.OnClickLoad();
    }

    public void OnClickOpenOption()
    {
        if (optionPanel != null)
        {
            SoundManager.Instance.PlaySFX(SoundType.ButtonClick);
            optionPanel.SetActive(true);
        }
    }

    public void OnClickCloseOption()
    {
        if (optionPanel != null)
        {
            SoundManager.Instance.PlaySFX(SoundType.ButtonClick);
            optionPanel.SetActive(false);
        }
    }

    public void OnClickExit()
    {
        SoundManager.Instance.PlaySFX(SoundType.ButtonClick);
        UIManager.Instance?.OnClickExit();
    }
}