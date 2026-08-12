using System;
using System.Collections.Generic;
using Restaurant.Foods;

namespace Restaurant.Orders
{
    [Serializable]
    public class OrderItem
    {
        public FoodDataSO food;
        public int requiredAmount; // 필요 개수
        public int currentAmount;  // 현재 서빙된 개수

        public OrderItem(FoodDataSO food, int amount)
        {
            this.food = food;
            this.requiredAmount = amount;
            this.currentAmount = 0;
        }

        public bool IsFulfilled => currentAmount >= requiredAmount;
    }

    [Serializable]
    public class OrderData
    {
        public string orderID;           // 주문 고유 ID
        public int tableIndex;           // 의자(Chair)의 GetInstanceID()? 이렇게 하면 되나?
        public List<OrderItem> orderItems; // 무작위 메뉴 조합 리스트
        public OrderStatus status;       // 주문 상태

        public OrderData(int tableIndex, List<OrderItem> items)
        {
            this.orderID = Guid.NewGuid().ToString();
            this.tableIndex = tableIndex;
            this.orderItems = items;
            this.status = OrderStatus.Waiting;
        }

        public bool IsFulfilled
        {
            get
            {
                if (orderItems == null || orderItems.Count == 0) return false;
                foreach (var item in orderItems)
                {
                    if (!item.IsFulfilled) return false;
                }
                return true;
            }
        }

        public int GetTotalFoodCount()
        {
            int total = 0;
            if (orderItems != null)
            {
                foreach (var item in orderItems)
                {
                    total += item.requiredAmount;
                }
            }
            return total;
        }

        public int GetTotalMoneyDropCount()
        {
            return GetTotalFoodCount() * 4; // 음식 하나 당 5머니짜리 4개.
        }
    }
}