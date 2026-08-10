using UnityEngine;

public class ReputationManager : MonoSingleton<ReputationManager>
{
    [Header("명성 설정")]
    [SerializeField] private int initialReputation = 0;

    public int CurrentReputation { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        CurrentReputation = initialReputation;
    }

    public void AddReputation(int amount)
    {
        if (amount <= 0) return;

        CurrentReputation += amount;
        Debug.Log($"[ReputationManager] 명성 획득: +{amount} (현재 명성: {CurrentReputation})");

        EventManager.Instance?.Publish(EventManager.EventType.OnReputationChanged);
    }

    public void DecreaseReputation(int amount)
    {
        if (amount <= 0) return;

        CurrentReputation = Mathf.Max(0, CurrentReputation - amount);
        Debug.Log($"[ReputationManager] 명성 감소: -{amount} (현재 명성: {CurrentReputation})");

        EventManager.Instance?.Publish(EventManager.EventType.OnReputationChanged);
    }
}