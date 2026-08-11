using System.Collections.Generic;
using UnityEngine;
using Restaurant.Foods;

namespace Restaurant.Orders
{
    public class OrderManager : MonoSingleton<OrderManager>
    {
        private Dictionary<int, OrderData> activeOrders = new Dictionary<int, OrderData>();

        protected override void Awake()
        {
            base.Awake();
        }

        public void RegisterOrder(OrderData newOrder)
        {
            if (newOrder == null) return;

            if (activeOrders.ContainsKey(newOrder.tableIndex))
            {
                Debug.LogWarning($"[OrderManager] {newOrder.tableIndex}번 테이블에 이미 주문이 존재합니다.");
                return;
            }

            activeOrders.Add(newOrder.tableIndex, newOrder);
            Debug.Log($"[OrderManager] {newOrder.tableIndex}번 테이블 주문 등록 완료: {newOrder.targetFood.foodName} x{newOrder.requiredAmount}");

            EventManager.Instance?.Publish(EventType.OnOrderCreated, newOrder);
        }

        public bool ServeFoodToTable(int tableIndex, FoodDataSO food)
        {
            if (!activeOrders.TryGetValue(tableIndex, out OrderData order))
            {
                return false;
            }

            if (order.status != OrderStatus.Waiting)
            {
                return false;
            }

            if (order.targetFood.foodID == food.foodID)
            {
                order.currentAmount++;
                Debug.Log($"[OrderManager] {tableIndex}번 테이블 서빙 성공 ({order.currentAmount}/{order.requiredAmount})");

                EventManager.Instance?.Publish(EventType.OnOrderUpdated, order);

                if (order.IsFulfilled)
                {
                    CompleteOrder(tableIndex);
                }

                return true;
            }

            return false;
        }

        private void CompleteOrder(int tableIndex)
        {
            if (activeOrders.TryGetValue(tableIndex, out OrderData order))
            {
                order.status = OrderStatus.Completed;

                int earnedMoney = order.targetFood.price * order.requiredAmount;
                CurrencyManager.Instance?.AddMoney(earnedMoney);

                Debug.Log($"[OrderManager] {tableIndex}번 테이블 주문 완료! 획득 Money: {earnedMoney}");

                EventManager.Instance?.Publish(EventType.OnOrderCompleted, order);
                activeOrders.Remove(tableIndex);
            }
        }

        public void CancelOrder(int tableIndex)
        {
            if (activeOrders.TryGetValue(tableIndex, out OrderData order))
            {
                order.status = OrderStatus.Canceled;
                EventManager.Instance?.Publish(EventType.OnOrderCompleted, order);
                activeOrders.Remove(tableIndex);
            }
        }

        public OrderData GetOrder(int tableIndex)
        {
            activeOrders.TryGetValue(tableIndex, out OrderData order);
            return order;
        }
    }
}