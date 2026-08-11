using UnityEngine;

public class CurrencyManager : MonoSingleton<CurrencyManager>
{
    [Header("재화 설정")]
    [SerializeField] private int initialGold = 0;
    [SerializeField] private int initialMoney = 0;
    [SerializeField] private int initialGems = 0;

    public int CurrentGold { get; private set; }
    public int CurrentMoney { get; private set; }
    public int CurrentGems { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        CurrentGold = initialGold;
        CurrentMoney = initialMoney;
        CurrentGems = initialGems;
    }

    public void ResetData()
    {
        CurrentGold = initialGold;
        CurrentMoney = initialMoney;
        CurrentGems = initialGems;
        Debug.Log($"[CurrencyManager] 데이터 초기화: Gold {CurrentGold}, Money {CurrentMoney}, Gems {CurrentGems}");

        EventManager.Instance?.Publish(EventType.OnGoldChanged, CurrentGold);
        EventManager.Instance?.Publish(EventType.OnMoneyChanged, CurrentMoney);
    }

    public void SetMoney(int amount)
    {
        CurrentMoney = Mathf.Max(0, amount);
        EventManager.Instance?.Publish(EventType.OnMoneyChanged, CurrentMoney);
    }

    public void AddMoney(int amount)
    {
        if (amount <= 0) return;

        CurrentMoney += amount;
        Debug.Log($"[CurrencyManager] 돈 획득: +{amount} (현재 Money: {CurrentMoney})");

        EventManager.Instance?.Publish(EventType.OnMoneyChanged, CurrentMoney);
    }

    public bool HasEnoughMoney(int amount)
    {
        return CurrentMoney >= amount;
    }

    public bool TrySpendMoney(int amount)
    {
        if (amount <= 0) return false;

        if (HasEnoughMoney(amount))
        {
            CurrentMoney -= amount;
            Debug.Log($"[CurrencyManager] 돈 소비: -{amount} (현재 Money: {CurrentMoney})");

            EventManager.Instance?.Publish(EventType.OnMoneyChanged, CurrentMoney);
            return true;
        }

        Debug.LogWarning($"[CurrencyManager] 돈 부족! (필요: {amount}, 현재: {CurrentMoney})");
        return false;
    }

            public void AddGold(int amount)
            {
                if (amount <= 0) return;

                CurrentGold += amount;
                Debug.Log($"[CurrencyManager] 골드 획득: +{amount} (현재 골드: {CurrentGold})");

                EventManager.Instance?.Publish(EventType.OnGoldChanged, CurrentGold);
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

                    EventManager.Instance?.Publish(EventType.OnGoldChanged, CurrentGold);
                    return true;
                }

                Debug.LogWarning($"[CurrencyManager] 골드 부족! (필요: {amount}, 현재: {CurrentGold})");
                return false;
            }
    
    public void AddGems(int amount)
    {
        if (amount <= 0) return;
        CurrentGems += amount;
    }

    public bool TrySpendGems(int amount)
    {
        if (amount <= 0) return false;
        if (CurrentGems >= amount)
        {
            CurrentGems -= amount;
            return true;
        }
        return false;
    }
}