using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Tycoon/Item Data")]
public class ItemDataSO : ScriptableObject
{
    [Header("Item Info")]
    [SerializeField] private string itemId;
    [SerializeField] private string itemName;
    [SerializeField] private float itemHeight = 0.1f; // 쌓일 때의 높이
    [SerializeField] private CarrierItem itemPrefab;  // 실제로 생성될 프리팹

    public string ItemId => itemId;
    public string ItemName => itemName;
    public float ItemHeight => itemHeight;
    public CarrierItem ItemPrefab => itemPrefab;
}
