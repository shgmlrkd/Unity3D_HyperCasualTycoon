using UnityEngine;

namespace Restaurant.Foods
{
    //[CreateAssetMenu(fileName = "FoodDataSO", menuName = "Scriptable Objects/FoodDataSO")]
    [CreateAssetMenu(fileName = "FoodData_", menuName = "Restaurant/Food Data")]
    public class FoodDataSO : ScriptableObject
    {
        [Header("Food Info")]
        public string foodID;          // 음식 고유 ID (예: "Burger", "Juice")
        public string foodName;        // 음식 이름
        public Sprite foodIcon;        // 테이블 위 UI 표시용 아이콘
        public int price;              // 음식 가격 (판매금)
        public bool isUnlocked = true; // 현재 판매 가능 여부
    }
}