using System.Collections.Generic;
using UnityEngine;
using Restaurant.Foods;

namespace Restaurant.Orders
{
    public class OrderGenerator : MonoBehaviour
    {
        [Header("판매 가능한 전체 음식 목록")]
        [SerializeField] private List<FoodDataSO> availableFoods = new List<FoodDataSO>();

        public OrderData CreateRandomOrder(int tableIndex, int minAmount = 1, int maxAmount = 2)
        {
            List<FoodDataSO> unlockedFoods = GetUnlockedFoods();

            if (unlockedFoods.Count == 0)
            {
                Debug.LogError("[OrderGenerator] 현재 해금된 음식 데이터가 없습니다!");
                return null;
            }

            FoodDataSO selectedFood = unlockedFoods[Random.Range(0, unlockedFoods.Count)];
            int randomAmount = Random.Range(minAmount, maxAmount + 1);

            OrderData newOrder = new OrderData(tableIndex, selectedFood, randomAmount);

            if (OrderManager.Instance != null)
            {
                OrderManager.Instance.RegisterOrder(newOrder);
            }

            return newOrder;
        }

        private List<FoodDataSO> GetUnlockedFoods()
        {
            List<FoodDataSO> unlocked = new List<FoodDataSO>();
            foreach (var food in availableFoods)
            {
                if (food != null && food.isUnlocked)
                {
                    unlocked.Add(food);
                }
            }
            return unlocked;
        }
    }
}