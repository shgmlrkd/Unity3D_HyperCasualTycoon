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
            // 손님 NPC 스폰 시키기
            TrySpawnCustomer();

            yield return waitForSpawn;
        }
    }

    private void TrySpawnCustomer()
    {
        // 이건 아직까진 보류 필요 없을 확률 있음
        if (currentCustomerCount >= maxCustomerCount)
            return;

        // 사용 가능한 의자가 없으면 리턴
        if (!hasAvailableChair)
            return;

        // 손님 NPC 스폰
        SpawnCustomer();
    }

    // 랜덤한 위치에서 손님 NPC 스폰 후 사용 가능한 의자를 목적지로 삼아 이동
    private void SpawnCustomer()
    {
        int index = Random.Range(0, spawnPosData.Positions.Count);

        CustomerNPC customer = PoolManager.Instance.Pop<CustomerNPC>(PoolType.Customer);

        if (customer == null)
            return;

        // 아래처럼 했음에도 원점에서 스폰되는 버그가 발견
        //customer.transform.position = spawnPosData.Positions[index];
        //print($"현재 스폰 위치 : {customer.transform.position}");

        // 따라서 이렇게 바꿈 -> Navmesh를 통한 순간이동으로 해당 위치에 스폰
        customer.MoveController.ResetAgent(spawnPosData.Positions[index]);

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