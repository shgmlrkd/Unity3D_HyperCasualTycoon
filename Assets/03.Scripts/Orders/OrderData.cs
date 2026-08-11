using System;
using Restaurant.Foods;

namespace Restaurant.Orders
{
    [Serializable]
    public class OrderData
    {
        public string orderID;           // 주문 고유 번호 or ID
        public int tableIndex;           // 주문이 발생한 테이블 번호나 ID
        public FoodDataSO targetFood;    // 주문한 음식 정보
        public int requiredAmount;       // 필요 개수
        public int currentAmount;        // 현재까지 서빙된 개수
        public OrderStatus status;       // 현재 주문 상태

        public OrderData(int tableIndex, FoodDataSO food, int amount = 1)
        {
            this.orderID = Guid.NewGuid().ToString();
            this.tableIndex = tableIndex;
            this.targetFood = food;
            this.requiredAmount = amount;
            this.currentAmount = 0;
            this.status = OrderStatus.Waiting;
        }

        public bool IsFulfilled => currentAmount >= requiredAmount;
    }
}