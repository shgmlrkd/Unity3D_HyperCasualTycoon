using UnityEngine;

public class TitleUIController : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject optionPanel;

    public void OnClickNewGame()
    {
        SoundManager.Instance.PlaySFX();
        UIManager.Instance?.OnClickNewGame();
    }

    public void OnClickLoad()
    {
        SoundManager.Instance.PlaySFX();
        UIManager.Instance?.OnClickLoad();
    }

    public void OnClickOpenOption()
    {
        if (optionPanel != null)
        {
            SoundManager.Instance.PlaySFX();
            optionPanel.SetActive(true);
        }
    }

    public void OnClickCloseOption()
    {
        if (optionPanel != null)
        {
            SoundManager.Instance.PlaySFX();
            optionPanel.SetActive(false);
        }
    }

    public void OnClickExit()
    {
        SoundManager.Instance.PlaySFX();
        UIManager.Instance?.OnClickExit();
    }
}