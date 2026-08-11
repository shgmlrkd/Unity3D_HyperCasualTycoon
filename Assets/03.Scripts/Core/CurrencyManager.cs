using UnityEngine;

public class CurrencyManager : MonoSingleton<CurrencyManager>
{
    [Header("재화 설정")]
    [SerializeField] private int initialGold = 0;

    public int CurrentGold { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        CurrentGold = initialGold;
    }

    public void ResetData()
    {
        CurrentGold = initialGold;
        Debug.Log($"[CurrencyManager] 데이터 초기화: 골드 {CurrentGold}으로 리셋");

        EventManager.Instance?.Publish(EventType.OnGoldChanged, CurrentGold);
    }

    public void AddGold(int amount)
    {
        if (amount <= 0) return;

        CurrentGold += amount;
        Debug.Log($"[CurrencyManager] 골드 획득: +{amount} (현재 골드: {CurrentGold})");

        EventManager.Instance?.Publish(EventType.OnGoldChanged);
    }

    public bool HasEnoughGold(int amount)
    {
        return CurrentGold >= amount;
    }

    public bool TrySpendGold(int amount)
    {
        if (amount <= 0) return false;

        if (HasEnoughGold(amount))
        {
            CurrentGold -= amount;
            Debug.Log($"[CurrencyManager] 골드 소비: -{amount} (현재 골드: {CurrentGold})");

            EventManager.Instance?.Publish(EventType.OnGoldChanged);
            return true;
        }

        Debug.LogWarning($"[CurrencyManager] 골드 부족! (필요: {amount}, 현재: {CurrentGold})");
        return false;
    }
}