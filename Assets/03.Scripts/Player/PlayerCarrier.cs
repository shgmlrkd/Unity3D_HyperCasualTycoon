using System.Collections.Generic;
using UnityEngine;

public class PlayerCarrier : MonoBehaviour
{
    [Header("Stacking Settings")]
    [SerializeField] private Transform carryPoint;
    [SerializeField] private int maxCapacity = 5;
    [SerializeField] private float itemSpacing = 0.05f;

    private Stack<CarrierItem> itemStack = new Stack<CarrierItem>();

    public int CurrentCount => itemStack.Count;
    public int MaxCapacity => maxCapacity;
    public bool IsFull => itemStack.Count >= maxCapacity;
    public bool HasItems => itemStack.Count > 0;

    // 기존 메서드 (단순 생성)
    public bool TryAddCarrierItem(CarrierItem itemPrefab)
    {
        return TryAddCarrierItem(itemPrefab, carryPoint.position);
    }

    // 스폰 위치(spawnWorldPosition)를 받아 생성하는 오버로딩 메서드
    public bool TryAddCarrierItem(CarrierItem itemPrefab, Vector3 spawnWorldPosition)
    {
        if (IsFull) return false;

        // Y축 높이 계산
        float currentYOffset = 0f;
        foreach (var item in itemStack)
        {
            currentYOffset += item.ItemHeight + itemSpacing;
        }

        // 스폰 지점 위치에 생성 후 carryPoint의 자식으로 등록
        CarrierItem newItem = Instantiate(itemPrefab, spawnWorldPosition, Quaternion.identity);
        newItem.transform.SetParent(carryPoint);

        // 머리 위 목표 상대 위치 설정 (추후 DOTween 애니메이션 연출 적용 가능 구역)
        newItem.transform.localPosition = new Vector3(0, currentYOffset, 0);
        newItem.transform.localRotation = Quaternion.identity;

        itemStack.Push(newItem);
        return true;
    }

    public CarrierItem PopCarrierItem()
    {
        if (!HasItems) return null;

        CarrierItem item = itemStack.Pop();
        item.transform.SetParent(null);
        return item;
    }
    // 들고 있는 모든 아이템 삭제 (쓰레기통 이용 시)
    public void ClearAllItems()
    {
        while (itemStack.Count > 0)
        {
            CarrierItem item = itemStack.Pop();
            Destroy(item.gameObject); // 프리팹 오브젝트 완전 파괴
        }
    }
}
