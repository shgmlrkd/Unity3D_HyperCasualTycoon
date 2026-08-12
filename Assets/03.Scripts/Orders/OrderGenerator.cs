using System.Collections.Generic;
using UnityEngine;
using Restaurant.Foods;

namespace Restaurant.Orders
{
    public class OrderGenerator : MonoSingleton<OrderGenerator>
    {
        [Header("피햄 건물 음식 리스트")]
        [SerializeField] private List<FoodDataSO> pizzaHamburgerFoods = new List<FoodDataSO>();

        [Header("케아 건물 음식 리스트")]
        [SerializeField] private List<FoodDataSO> cakeIcecreamFoods = new List<FoodDataSO>();

        public OrderData CreateRandomOrder(int chairID, RestaurantType restaurantType, int maxMenuTypes = 2, int maxAmountPerMenu = 2)
        {
            List<FoodDataSO> targetFoods = null;

            switch (restaurantType)
            {
                case RestaurantType.PizzaHamburger:
                    targetFoods = pizzaHamburgerFoods;
                    break;
                case RestaurantType.CakeIcecream:
                    targetFoods = cakeIcecreamFoods;
                    break;
                default:
                    Debug.LogError($"[OrderGenerator] 유효하지 않은 RestaurantType: {restaurantType}");
                    return null;
            }

            if (targetFoods == null || targetFoods.Count == 0)
            {
                Debug.LogError($"[OrderGenerator] {restaurantType} 건물에 등록된 음식 SO가 없습니다!");
                return null;
            }

            List<OrderItem> orderItems = new List<OrderItem>();
            List<FoodDataSO> pool = new List<FoodDataSO>(targetFoods);

            int menuTypeCount = Random.Range(1, Mathf.Min(maxMenuTypes, pool.Count) + 1);

            for (int i = 0; i < menuTypeCount; i++)
            {
                int randomIndex = Random.Range(0, pool.Count);
                FoodDataSO selectedFood = pool[randomIndex];
                pool.RemoveAt(randomIndex); // 중복 메뉴 방지

                int amount = Random.Range(1, maxAmountPerMenu + 1); // 음식당 1~2개 무작위
                orderItems.Add(new OrderItem(selectedFood, amount));
            }

            OrderData newOrder = new OrderData(chairID, orderItems);

            if (OrderManager.Instance != null)
            {
                OrderManager.Instance.RegisterOrder(newOrder);
            }

            return newOrder;
        }
    }
}