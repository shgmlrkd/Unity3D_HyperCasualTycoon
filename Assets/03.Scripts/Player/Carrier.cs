using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : MonoBehaviour
{
    [Header("Stacking Settings")]
    [SerializeField] private Transform carryPoint;
    private int maxCapacity = 0;
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

    public void SetMaxCapacity(int maxCapacity)
    {
        this.maxCapacity = maxCapacity;
    }

    // 스폰 위치를 지정하여 생성하는 메서드
    public bool TryAddCarrierItem(CarrierItem itemPrefab, Vector3 spawnWorldPosition)
    {
        if (IsFull || itemPrefab == null) return false;

        // List를 순회하여 현재 Y축 높이 계산
        float currentYOffset = 0.0f;
        for (int i = 0; i < itemList.Count; i++)
        {
            currentYOffset += itemList[i].ItemHeight + itemSpacing;
        }

        // 스폰 지점에 생성 후 carryPoint 자식으로 등록
        CarrierItem newItem = Instantiate(itemPrefab, spawnWorldPosition, Quaternion.identity);
        Transform targetParent = carryPoint != null ? carryPoint : transform;
        newItem.transform.SetParent(targetParent);

        // 머리 위/손 로컬 위치 및 회전 설정
        Vector3 endPos = new Vector3(0.0f, currentYOffset, 0.0f);
        newItem.transform.DOLocalJump(endPos, 1.0f, 1, 0.15f);
        newItem.transform.localRotation = Quaternion.identity;

        SoundManager.Instance.PlaySFX(SoundType.Food);

        itemList.Add(newItem);
        circleGauge.StartGauge();

        OnItemCountChanged?.Invoke(itemList.Count);
        return true;
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

    public CarrierItem GetOrderItem(FoodType foodType)
    {
        if (itemList.Count == 0) return null;

        for(int i = itemList.Count - 1; i >= 0; i--)
        {
            if (foodType != itemList[i].ItemId)
                continue;
            
            CarrierItem item = itemList[i];
            itemList.RemoveAt(i);

            if (item != null)
            {
                item.transform.SetParent(null);
            }

            RealignStackPositions(i); // 중간 아이템이 빠진 경우 위치 재정렬
            OnItemCountChanged?.Invoke(itemList.Count);

            return item;
            
        }

        return null;
    }

    // 중간 아이템이 제거되었을 때 남은 아이템들의 위치를 차곡차곡 재정렬
    private void RealignStackPositions(int startIndex)
    {
        float currentYOffset = 0.0f;

        // startIndex 이전의 아이템들은 기존 위치 유지
        for (int i = 0; i < startIndex; i++)
        {
            if (itemList[i] == null)
                continue;

            currentYOffset += itemList[i].ItemHeight + itemSpacing;
        }

        // 제거된 위치 이후의 아이템만 재정렬
        for (int i = startIndex; i < itemList.Count; i++)
        {
            if (itemList[i] == null)
                continue;

            itemList[i].transform.DOKill();

            Vector3 targetPosition = new Vector3(0.0f, currentYOffset, 0.0f);

            itemList[i].transform.DOLocalMove(targetPosition, 0.2f);

            currentYOffset += itemList[i].ItemHeight + itemSpacing;
        }
    }

    public bool HasFood(FoodType foodType)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].ItemId == foodType)
                return true;
        }

        return false;
    }
}