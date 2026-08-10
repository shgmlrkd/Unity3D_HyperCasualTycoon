using UnityEngine;

public class TitleUIController : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject optionPanel;

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

    public void OnClickExit()
    {
        UIManager.Instance?.OnClickExit();
    }
}