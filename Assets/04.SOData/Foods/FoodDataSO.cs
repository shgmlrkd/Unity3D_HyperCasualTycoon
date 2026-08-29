using UnityEngine;

namespace Restaurant.Foods
{

    [CreateAssetMenu(fileName = "FoodData_", menuName = "Restaurant/Food Data")]
    public class FoodDataSO : ScriptableObject
    {
        [Header("Food Info")]
        public FoodType foodID;          // 음식 고유 ID (예: "Burger", "Pizza")
        public string foodName;        // 음식 이름
        public Sprite foodIcon;        // UI 표시용 아이콘
        public CarrierItem foodPrefab;

        [Header("Restaurant Building")]
        public RestaurantType restaurantType; // 해당 음식이 판매되는 건물

    }
}