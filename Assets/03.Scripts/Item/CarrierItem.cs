using UnityEngine;

public class CarrierItem : MonoBehaviour
{
    [Header("Item Info")]
    [SerializeField] private FoodType itemId;
    [SerializeField] private float itemHeight = 0.03f; // 이 아이템의 높이 (다음 아이템이 위에 쌓일 거리)

    public FoodType ItemId => itemId;
    public float ItemHeight => itemHeight;
}