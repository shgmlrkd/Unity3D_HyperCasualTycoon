using UnityEngine;

public class MoneyManager : LocalSingleton<MoneyManager>
{
    [Header("Money")]
    [SerializeField]
    private Money moneyPrefab;

    [SerializeField]
    private int moneyPoolSize = 100;

    [Header("Drop")]
    [SerializeField] private int dropCount = 4;
    [SerializeField] private float dropRadius = 1.0f;
    [SerializeField] private float dropHeight = 0.1f;

    private void Awake()
    {
        base.Awake();

        CreateMoneyPool();
    }

    private void CreateMoneyPool()
    {
        PoolManager.Instance.CreatePool(PoolType.Money, moneyPrefab, moneyPoolSize);
    }

    public void PayMoney(Vector3 spawnPosition)
    {
        for (int i = 0; i < dropCount; i++)
        {
            Money money = PoolManager.Instance.Pop<Money>(PoolType.Money);

            Vector3 randomOffset = new Vector3(Random.Range(-dropRadius, dropRadius), dropHeight, Random.Range(-dropRadius, dropRadius));

            Vector3 targetPosition = spawnPosition + randomOffset;

            money.Spawn(spawnPosition);
            money.Drop(targetPosition);
        }

        SoundManager.Instance.PlaySFX(SoundType.Money);
    }
}