using System.Collections.Generic;
using UnityEngine;
using Restaurant.Foods;

namespace Restaurant.Orders
{
    public class OrderManager : MonoSingleton<OrderManager>
    {
        // Key: 의자(Chair)의 InstanceID
        private Dictionary<int, OrderData> activeOrders = new Dictionary<int, OrderData>();

        public void RegisterOrder(OrderData newOrder)
        {
            if (newOrder == null) return;

            if (activeOrders.ContainsKey(newOrder.tableIndex))
            {
                Debug.LogWarning($"[OrderManager] Chair ID {newOrder.tableIndex} 위치에 이미 주문이 존재합니다.");
                return;
            }

            activeOrders.Add(newOrder.tableIndex, newOrder);
            Debug.Log($"[OrderManager] Chair ID {newOrder.tableIndex} 주문 등록 완료");

            EventManager.Instance?.Publish(EventType.OnOrderCreated, newOrder);
        }

        public bool ServeFoodToTable(int chairID, FoodDataSO food)
        {
            if (!activeOrders.TryGetValue(chairID, out OrderData order))
            {
                return false;
            }

            if (order.status != OrderStatus.Waiting)
            {
                return false;
            }

            foreach (var item in order.orderItems)
            {
                if (item.food.foodID == food.foodID && !item.IsFulfilled)
                {
                    item.currentAmount++;
                    Debug.Log($"[OrderManager] 서빙 성공: {food.foodName} ({item.currentAmount}/{item.requiredAmount})");

                    EventManager.Instance?.Publish(EventType.OnOrderUpdated, order);

                    if (order.IsFulfilled)
                    {
                        CompleteOrder(chairID);
                    }

                    return true;
                }
            }

            return false;
        }

        private void CompleteOrder(int chairID)
        {
            if (activeOrders.TryGetValue(chairID, out OrderData order))
            {
                order.status = OrderStatus.Completed;
                Debug.Log($"[OrderManager] Chair ID {chairID} 모든 음식 서빙 완료!");

                EventManager.Instance?.Publish(EventType.OnOrderCompleted, order);
                activeOrders.Remove(chairID);
            }
        }

        public OrderData GetOrder(int chairID)
        {
            activeOrders.TryGetValue(chairID, out OrderData order);
            return order;
        }
    }
}