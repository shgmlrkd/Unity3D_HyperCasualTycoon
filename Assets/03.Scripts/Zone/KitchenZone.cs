using System.Threading;
using UnityEngine;

public class KitchenZone : MonoBehaviour
{
    [Header("Zone Data")]
    [SerializeField] private ItemDataSO produceItemData; // 이 구역에서 나올 특정 음식 SO (예: 피자 또는 햄버거)
    [SerializeField] private float interactInterval = 1.0f; // 생산 주기(초)

    [Header("Spawn Point")]
    [SerializeField] private Transform spawnPoint; // 이 음식만 나올 전용 스폰 지점

    private float timer = 0f;

    public void ResetTimer()
    {
        timer = 0f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent(out Carrier carrier)) return;

        timer += Time.deltaTime;

        if (timer >= interactInterval)
        {
            timer = 0.0f;

            // 지정된 스폰 위치(없으면 현재 오브젝트 위치)에서 음식 생성 후 전달
            Vector3 spawnPos = (spawnPoint != null) ? spawnPoint.position : transform.position;
            carrier.TryAddCarrierItem(produceItemData.ItemPrefab, spawnPos);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out Carrier carrier)) return;

        timer = 0.0f;
    }
}