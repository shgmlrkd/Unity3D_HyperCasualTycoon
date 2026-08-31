using DG.Tweening;
using Restaurant.Orders;
using System.Collections.Generic;
using UnityEngine;

public class OrderUIManager : LocalSingleton<OrderUIManager>
{
    [SerializeField]
    private Order orderPrefab;

    [SerializeField]
    private int orderUIPoolSize = 100;

    [SerializeField]
    private float spacing = 1.0f;

    private float orderUIOffsetY = 1.9f;
    private float orderUIOffsetZ = -0.3f;

    private Dictionary<int, List<Order>> orderUIs = new Dictionary<int, List<Order>>();

    private void Awake()
    {
        base.Awake();

        CreateOrderUIPool();
    }

    private void OnEnable()
    {
        if (OrderManager.Instance == null) return;

        OrderManager.Instance.OnChangeOrderItem += UpdateCustomerOrderUI;
    }

    private void OnDisable()
    {
        if (OrderManager.Instance == null) return;

        OrderManager.Instance.OnChangeOrderItem -= UpdateCustomerOrderUI;
    }

    private void CreateOrderUIPool()
    {
        PoolManager.Instance.CreatePool(PoolType.Order, orderPrefab, orderUIPoolSize);
    }

    // 손님 NPC 주문 UI를 생성하고 초기 위치를 설정
    public void SetCustomerOrderUI(OrderData data, Transform transform)
    {
        int index = 0;

        List<Order> orderUIList = new List<Order>();

        foreach(OrderItem item in data.orderItems)
        {
            Order order = PoolManager.Instance.Pop<Order>(PoolType.Order);

            order.transform.DOKill();
            order.SetOrderInfo(item.food.foodIcon, item.requiredAmount);

            Vector3 position = transform.position;

            position.y += orderUIOffsetY + (spacing * index);
            position += transform.forward * orderUIOffsetZ;

            order.transform.position = position;

            orderUIList.Add(order);

            index++;
        }

        orderUIs[data.customerID] = orderUIList;
    }

    // 변경된 주문 정보를 해당 주문 UI에 반영
    public void UpdateCustomerOrderUI(int customerID, OrderItem item)
    {
        List<Order> orderList = orderUIs[customerID];

        for (int i = 0; i < orderList.Count; i++)
        {
            Order order = orderList[i];

            if (item.food.foodIcon != orderList[i].FoodSprite) continue;

            order.UpdateOrderInfo(item.RemainAmount);

            if (item.RemainAmount == 0)
            {
                RemoveOrderUI(orderList, i);
            }

            return;
        }
    }

    // 완료된 주문 UI를 제거하고 이후 주문 UI를 재배치
    private void RemoveOrderUI(List<Order> orderList, int removeIndex)
    {
        Vector3 removePosition = orderList[removeIndex].transform.position;

        for (int i = removeIndex + 1; i < orderList.Count; i++)
        {
            MoveOrderUI(orderList[i], removePosition);
            removePosition = orderList[i].transform.position;
        }

        orderList.RemoveAt(removeIndex);
    }

    // 주문 UI를 갱신하면서 DOMove로 지정된 위치로 이동
    private void MoveOrderUI(Order order, Vector3 targetPosition)
    {
        order.transform.DOKill();
        order.transform.DOMove(targetPosition, 0.2f);
    }
}
