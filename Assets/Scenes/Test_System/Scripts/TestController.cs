using UnityEngine;

public class TestController : MonoBehaviour
{
    private void OnEnable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.Subscribe(EventType.OnGoldChanged, OnGoldChangedHandler);
            EventManager.Instance.Subscribe(EventType.OnReputationChanged, OnReputationChangedHandler);
            EventManager.Instance.Subscribe(EventType.OnGameStateChanged, OnGameStateChangedHandler);
        }
    }

    private void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.Unsubscribe(EventType.OnGoldChanged, OnGoldChangedHandler);
            EventManager.Instance.Unsubscribe(EventType.OnReputationChanged, OnReputationChangedHandler);
            EventManager.Instance.Unsubscribe(EventType.OnGameStateChanged, OnGameStateChangedHandler);
        }
    }

    private void Update()
    {
        // G키: 골드 +50 획득
        if (Input.GetKeyDown(KeyCode.G))
        {
            CurrencyManager.Instance?.AddGold(50);
        }

        // F키: 골드 -50 소비
        if (Input.GetKeyDown(KeyCode.F))
        {
            CurrencyManager.Instance?.TrySpendGold(50);
        }

        // R키: 명성 +5 획득
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReputationManager.Instance?.AddReputation(5);
        }

        // E키: 명성 -5 감소
        if (Input.GetKeyDown(KeyCode.E))
        {
            ReputationManager.Instance?.DecreaseReputation(5);
        }

        // P키: 일시정지 / 재개 토글
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (GameManager.Instance.CurrentState == GameState.Play)
            {
                GameManager.Instance.PauseGame();
            }
            else if (GameManager.Instance.CurrentState == GameState.Pause)
            {
                GameManager.Instance.ResumeGame();
            }
        }

        // S키: 데이터 저장
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveManager.Instance?.SaveGameData();
        }

        // L키: 데이터 불러오기
        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveManager.Instance?.LoadGameData();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.CurrentState == GameState.Play)
            {
                GameManager.Instance.PauseGame();
            }
            else if (GameManager.Instance.CurrentState == GameState.Pause)
            {
                GameManager.Instance.ResumeGame();
            }
        }
    }

    private void OnGoldChangedHandler(object param)
    {
        Debug.Log($"<color=yellow>[이벤트]</color> 골드 변경! 현재 골드: {CurrencyManager.Instance.CurrentGold}");
    }

    private void OnReputationChangedHandler(object param)
    {
        Debug.Log($"<color=cyan>[이벤트]</color> 명성 변경! 현재 명성: {ReputationManager.Instance.CurrentReputation}");
    }

    private void OnGameStateChangedHandler(object param)
    {
        Debug.Log($"<color=green>[이벤트]</color> 게임 상태 변경! 현재 상태: {param}");
    }
}