using System.Collections;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [Header("Customer")]
    [SerializeField]
    private CustomerNPC customerPrefab;

    [SerializeField]
    private int customerPoolSize = 10;

    [SerializeField]
    private int maxCustomerCount = 1;

    [SerializeField]
    private float spawnInterval = 3.0f;

    [Header("Spawn Points")]
    [SerializeField]
    private Transform[] spawnPoints = new Transform[4];

    [Header("ChairEventChannel")]
    [SerializeField]
    private ChairEventData chairEventChannel;

    private int currentCustomerCount;

    private WaitForSeconds waitForSpawn;

    private void Awake()
    {
        CreateCustomerPool();

        waitForSpawn = new WaitForSeconds(spawnInterval);
    }

    private void Start()
    {
        StartCoroutine(SpawnCustomerRoutine());
    }

    private void CreateCustomerPool()
    {
        PoolManager.Instance.CreatePool(PoolType.Customer, customerPrefab, customerPoolSize);
    }

    private IEnumerator SpawnCustomerRoutine()
    {
        while (true)
        {
            if (currentCustomerCount < maxCustomerCount)
            {
                SpawnCustomer();
            }

            yield return waitForSpawn;
        }
    }

    private void SpawnCustomer()
    {
        int index = Random.Range(0, spawnPoints.Length);

        CustomerNPC customer = PoolManager.Instance.Pop<CustomerNPC>(PoolType.Customer);

        if (customer == null)
            return;

        customer.transform.position = spawnPoints[index].position;

        currentCustomerCount++;

        SetupCustomer(customer);

        chairEventChannel.RequestChair(customer);
        //customer.Initialize(targetChair);
    }

    // 생성된 CustomerNPC의 종료 이벤트를 구독
    private void SetupCustomer(CustomerNPC customer)
    {
        customer.OnExitCompleted += OnCustomerExit;
    }

    // CustomerNPC의 퇴장 완료 이벤트 처리
    private void OnCustomerExit(CustomerNPC customer)
    {
        currentCustomerCount--;

        customer.OnExitCompleted -= OnCustomerExit;

        PoolManager.Instance.Release(PoolType.Customer, customer);
    }
}