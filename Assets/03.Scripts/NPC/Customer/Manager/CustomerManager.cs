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

    [Header("Spawn Position SO Data")]
    [SerializeField]
    private SpawnPositionData spawnPosData;

    [Header("ChairEventChannel")]
    [SerializeField]
    private ChairEventData chairEventChannel;

    private WaitForSeconds waitForSpawn;

    private int currentCustomerCount;
    private bool hasAvailableChair;

    private void Awake()
    {
        CreateCustomerPool();

        waitForSpawn = new WaitForSeconds(spawnInterval);
    }

    private void OnEnable()
    {
        chairEventChannel.OnChairAvailabilityChanged += HandleChairAvailabilityChanged;
    }

    private void Start()
    {
        StartCoroutine(SpawnCustomerRoutine());
    }

    private void OnDisable()
    {
        chairEventChannel.OnChairAvailabilityChanged -= HandleChairAvailabilityChanged;
    }

    private void CreateCustomerPool()
    {
        PoolManager.Instance.CreatePool(PoolType.Customer, customerPrefab, customerPoolSize);
    }

    private IEnumerator SpawnCustomerRoutine()
    {
        while (true)
        {
            TrySpawnCustomer();

            yield return waitForSpawn;
        }
    }

    private void TrySpawnCustomer()
    {
        if (currentCustomerCount >= maxCustomerCount)
            return;

        if (!hasAvailableChair)
            return;

        SpawnCustomer();
    }

    private void SpawnCustomer()
    {
        int index = Random.Range(0, spawnPosData.Positions.Count);

        CustomerNPC customer = PoolManager.Instance.Pop<CustomerNPC>(PoolType.Customer);

        if (customer == null)
            return;

        customer.transform.position = spawnPosData.Positions[index];

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

    // 의자 사용 가능 상태 변경 처리
    private void HandleChairAvailabilityChanged(bool isAvailable)
    {
        hasAvailableChair = isAvailable;
    }
}