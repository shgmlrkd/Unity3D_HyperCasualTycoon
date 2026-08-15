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
            // 매개변수로 FoodData가 없다면 false
            if (food == null)
            {
                return false;
            }
            
            // 이건 원래 있었던 코드입니다.
            if (!activeOrders.TryGetValue(chairID, out OrderData order))
            {
                return false;
            }

            // 이건 원래 있었던 코드입니다.
            if (order.status != OrderStatus.Waiting)
            {
                return false;
            }

            // foreach문을 수정했습니다.
            foreach (var item in order.orderItems)
            {
                // 현재 매개변수로 들어온 음식 데이터만 확인하면 되기 때문에
                // 다른 음식이면 무시
                if (item.food.foodID != food.foodID)
                    continue;

                // 이미 필요한 수량을 모두 받았다면 무시
                if (item.IsFulfilled)
                    continue;

                // 음식 수량 증가
                item.currentAmount++;

                Debug.Log($"Chair ID {chairID} [OrderManager] 서빙 성공: {food.foodName} ({item.currentAmount}/{item.requiredAmount})");

                EventManager.Instance?.Publish(EventType.OnOrderUpdated, order);
                
                // 모든 주문 음식이 충족되었다면 주문 완료
                if (order.IsFulfilled)
                {
                    CompleteOrder(chairID);
                }

                return true;
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