using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public GameState CurrentState { get; private set; } = GameState.Init;

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        ChangeState(GameState.Play);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        ChangeState(GameState.Play);
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        Debug.Log($"[GameManager] 게임 상태 변경: {CurrentState}");

        EventManager.Instance?.Publish(EventType.OnGameStateChanged, CurrentState);
    }

    public void PauseGame()
    {
        if (CurrentState == GameState.Play)
        {
            Time.timeScale = 0f;
            ChangeState(GameState.Pause);
        }
    }

    public void ResumeGame()
    {
        if (CurrentState == GameState.Pause)
        {
            Time.timeScale = 1f;
            ChangeState(GameState.Play);
        }
    }
}