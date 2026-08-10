using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKitchenData", menuName = "Tycoon/Kitchen Data")]
public class KitchenDataSO : ScriptableObject
{
    [Header("Production Settings")]
    [SerializeField] private List<ItemDataSO> produceItemDataList = new List<ItemDataSO>(); // 여러 음식 SO 리스트
    [SerializeField] private float interactInterval = 0.3f; // 생산 주기(초)

    public List<ItemDataSO> ProduceItemDataList => produceItemDataList;
    public float InteractInterval => interactInterval;

    // 원하는 순서(인덱스)의 음식을 가져오는 함수
    public ItemDataSO GetItemData(int index)
    {
        if (index >= 0 && index < produceItemDataList.Count)
        {
            return produceItemDataList[index];
        }
        return null;
    }
}
