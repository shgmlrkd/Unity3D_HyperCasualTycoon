using UnityEngine;

public class TitleUIController : MonoBehaviour
{
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
        UIManager.Instance?.OnClickOpenOption();
    }

    public void OnClickExit()
    {
        UIManager.Instance?.OnClickExit();
    }
}