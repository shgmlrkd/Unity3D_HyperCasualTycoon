using UnityEngine;

public class MoneyDrop : MonoBehaviour
{
    private const int MONEY_PER_CHUNK = 5; // 한 덩어리당 5 Money
    private bool isCollected = false;

    [Header("획득 범위")]
    [SerializeField] private float pickupRadius = 1.2f;

    private void Update()
    {
        if (isCollected) return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRadius);
        foreach (var col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                CollectMoney();
                break;
            }
        }
    }

    private void CollectMoney()
    {
        isCollected = true;

        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.AddMoney(MONEY_PER_CHUNK);
        }

        Destroy(gameObject);
    }
}