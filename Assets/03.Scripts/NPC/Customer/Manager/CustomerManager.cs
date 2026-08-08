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

        if(index + 1 ==  spawnPoints.Length)
        {
            index = 0;
        }
        else
        {
            index++;
        }
        currentCustomerCount++;
        customer.Initialize(spawnPoints[index]);
    }

    public void OnCustomerExit(CustomerNPC customer)
    {
        currentCustomerCount--;

        PoolManager.Instance.Release(PoolType.Customer, customer);
    }
}