using UnityEngine;

public class CurrencyManager : MonoSingleton<CurrencyManager>
{
    [Header("재화 설정")]
    [SerializeField] private int initialGold = 120;
    [SerializeField] private int initialMoney = 121;
    //[SerializeField] private int initialGems = 0;

    public int CurrentGold { get; private set; }
    public int CurrentMoney { get; private set; }
    //public int CurrentGems { get; private set; }

    //protected override void Awake()
    //{
    //    base.Awake();
    //    //CurrentGold = initialGold;
    //    //CurrentMoney = initialMoney;

    //    // SaveManager에 남아있는(또는 준비된) CurrentData가 있다면 해당 액수로 시작
    //    if (SaveManager.Instance != null && SaveManager.Instance.CurrentData != null)
    //    {
    //        CurrentMoney = SaveManager.Instance.CurrentData.money;
    //        CurrentGold = SaveManager.Instance.CurrentData.gold;
    //    }
    //    else
    //    {
    //        CurrentGold = initialGold;
    //        CurrentMoney = initialMoney;
    //    }

    //    CurrentGems = initialGems;
    //}


    private void Start()
    {
        InitCurrencyData();
    }

    public void InitCurrencyData()
    {
        if(SaveManager.Instance != null && SaveManager.Instance.CurrentData != null)
        {
            //CurrentMoney = SaveManager.Instance.CurrentData.money;
            //CurrentGold = SaveManager.Instance.CurrentData.gold;
            ApplySaveData(SaveManager.Instance.CurrentData);
        }
        else
        {
            //CurrentMoney = initialMoney;
            //CurrentGold = initialGold;
            ResetData();
        }

        EventManager.Instance?.Publish(EventType.OnGoldChanged, CurrentGold);
        EventManager.Instance?.Publish(EventType.OnMoneyChanged, CurrentMoney);

        Debug.Log($"[CurrencyManager] 초기화 완료: Gold {CurrentGold}, Money {CurrentMoney}");
    }

    public void ResetData()
    {
        CurrentGold = initialGold;
        CurrentMoney = initialMoney;
        //CurrentGems = initialGems;
        Debug.Log($"[CurrencyManager] 데이터 초기화: Gold {CurrentGold}, Money {CurrentMoney}");

        EventManager.Instance?.Publish(EventType.OnGoldChanged, CurrentGold);
        EventManager.Instance?.Publish(EventType.OnMoneyChanged, CurrentMoney);
    }

    public void ApplySaveData(SaveData data)
    {
        if(data == null)
        {
            Debug.LogWarning("[CurrencyManager] 적용할 SaveData가 없슴당");
            ResetData();
            return;
        }

        CurrentMoney = Mathf.Max(0, data.money);
        CurrentGold = Mathf.Max(0, data.gold);

        EventManager.Instance?.Publish(EventType.OnMoneyChanged, CurrentMoney);
        EventManager.Instance?.Publish(EventType.OnGoldChanged, CurrentGold);

        Debug.Log($"[CurrencyManager] 세이브 데이터 적용 완료. " + $"Gold {CurrentGold}, Money {CurrentMoney}");
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
    
    //public void AddGems(int amount)
    //{
    //    if (amount <= 0) return;
    //    CurrentGems += amount;
    //}

    //public bool TrySpendGems(int amount)
    //{
    //    if (amount <= 0) return false;
    //    if (CurrentGems >= amount)
    //    {
    //        CurrentGems -= amount;
    //        return true;
    //    }
    //    return false;
    //}
}