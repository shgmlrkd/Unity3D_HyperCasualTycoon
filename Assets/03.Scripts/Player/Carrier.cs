using System;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : MonoBehaviour
{
    [Header("Stacking Settings")]
    [SerializeField] private Transform carryPoint;
    [SerializeField] private int maxCapacity = 5;
    [SerializeField] private float itemSpacing = 0.05f;
    private CircleGauge circleGauge;

    // Stack 대신 List로 변경 (인덱스 접근 및 순회 용이)
    private readonly List<CarrierItem> itemList = new List<CarrierItem>();

    // 들고 있는 아이템 수 변경 시 발생하는 이벤트 (플레이어/NPC 애니메이션 연동용)
    public event Action<int> OnItemCountChanged;

    public int CurrentCount => itemList.Count;
    public int MaxCapacity => maxCapacity;
    public bool IsFull => itemList.Count >= maxCapacity;
    public bool HasItems => itemList.Count > 0;
    public IReadOnlyList<CarrierItem> ItemList => itemList;

    private void Awake()
    {
        circleGauge = GetComponentInChildren<CircleGauge>(true);
    }

    // 기본 생성 메서드
    public bool TryAddCarrierItem(CarrierItem itemPrefab)
    {
        return TryAddCarrierItem(itemPrefab, carryPoint != null ? carryPoint.position : transform.position);
    }

    // 스폰 위치를 지정하여 생성하는 메서드
    public bool TryAddCarrierItem(CarrierItem itemPrefab, Vector3 spawnWorldPosition)
    {
        if (IsFull || itemPrefab == null) return false;

        // List를 순회하여 현재 Y축 높이 계산
        float currentYOffset = 0f;
        for (int i = 0; i < itemList.Count; i++)
        {
            currentYOffset += itemList[i].ItemHeight + itemSpacing;
        }

        // 스폰 지점에 생성 후 carryPoint 자식으로 등록
        CarrierItem newItem = Instantiate(itemPrefab, spawnWorldPosition, Quaternion.identity);
        Transform targetParent = carryPoint != null ? carryPoint : transform;
        newItem.transform.SetParent(targetParent);

        // 머리 위/손 로컬 위치 및 회전 설정
        newItem.transform.localPosition = new Vector3(0f, currentYOffset, 0f);
        newItem.transform.localRotation = Quaternion.identity;

        itemList.Add(newItem);
        circleGauge.StartGauge();

        OnItemCountChanged?.Invoke(itemList.Count);
        return true;
    }

    // 맨 위(마지막) 아이템 꺼내기 (LIFO 형태 구현)
    public CarrierItem PopCarrierItem()
    {
        if (!HasItems) return null;

        int lastIndex = itemList.Count - 1;
        CarrierItem item = itemList[lastIndex];
        itemList.RemoveAt(lastIndex);

        if (item != null)
        {
            item.transform.SetParent(null);
        }

        OnItemCountChanged?.Invoke(itemList.Count);
        return item;
    }

    // 특정 위치/종류의 아이템을 꺼낼 때 사용 (List의 장점 활용)
    public CarrierItem RemoveItemAt(int index)
    {
        if (index < 0 || index >= itemList.Count) return null;

        CarrierItem item = itemList[index];
        itemList.RemoveAt(index);

        if (item != null)
        {
            item.transform.SetParent(null);
        }

        RealignStackPositions(); // 중간 아이템이 빠진 경우 위치 재정렬
        OnItemCountChanged?.Invoke(itemList.Count);
        return item;
    }

    // 모든 아이템 삭제 (쓰레기통 등)
    public void ClearAllItems()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i] != null)
            {
                Destroy(itemList[i].gameObject);
            }
        }
        itemList.Clear();
        OnItemCountChanged?.Invoke(0);
    }

    // 중간 아이템이 제거되었을 때 남은 아이템들의 위치를 차곡차곡 재정렬
    private void RealignStackPositions()
    {
        float currentYOffset = 0f;
        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].transform.localPosition = new Vector3(0f, currentYOffset, 0f);
            currentYOffset += itemList[i].ItemHeight + itemSpacing;
        }
    }
}