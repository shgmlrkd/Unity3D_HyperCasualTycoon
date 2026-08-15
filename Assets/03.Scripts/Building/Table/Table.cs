using Restaurant.Orders;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{ 
    // 그릇을 놓을 위치 2개가 필요함
    // 어떤 손님이 무엇을 주문 했는지 알아야함
    // 플레이어 또는 직원 NPC와 어떠한 음식을 들고 있고
    // 그 음식들 중 손님 NPC가 주문한 음식과 일치하는게 있는지 알아야함
    // 일치하는게 있다면 그릇 위치에 놓아야하고
    // 손님 NPC에게 서빙이 되었다는 신호를 보내야함

    [SerializeField]
    private int restaurantId;

    [SerializeField]
    private Transform[] plateTransforms = new Transform[2];

    [SerializeField]
    private Transform[] chairTransforms = new Transform[2];

    // 좌석 위치별 현재 손님
    private Dictionary<ChairSide, CustomerNPC> customerSides = new Dictionary<ChairSide, CustomerNPC>();
    
    // 손님별 테이블 위 음식
    private Dictionary<ChairSide, List<CarrierItem>> servedItems = new Dictionary<ChairSide, List<CarrierItem>>();
    
    public IReadOnlyDictionary<ChairSide, CustomerNPC> CustomerSides => customerSides;
    
    public int RestaurantId => restaurantId;

    private void OnTriggerEnter(Collider other)
    {
        /*// 테이블에 손님이 들어온 경우 등록
        if (other.TryGetComponent(out CustomerNPC customerNPC))
        {
            RegisterCustomer(customerNPC);
        }*/

        Debug.Log($"[Table] TriggerEnter : {other.name}");

        if (other.TryGetComponent(out CustomerNPC customerNPC))
        {
            if (customerNPC.CurrentChair == null)
                return;

            // 현재 손님이 목표로 한 의자가
            // 이 테이블의 의자 중 하나인지 확인
            for (int i = 0; i < chairTransforms.Length; i++)
            {
                if (customerNPC.CurrentChair.transform == chairTransforms[i])
                {
                    Debug.Log(
                        $"[Table] 올바른 의자 확인 : {customerNPC.name}, " +
                        $"ChairSide = {customerNPC.CurrentChair.SeatSide}");

                    RegisterCustomer(customerNPC);
                    break;
                }
            }
        }

        // 테이블에 플레이어 or 직원 NPC가 들어온 경우 서빙
        if (other.TryGetComponent(out Carrier carrier))
        {
            ServeFood(carrier);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out CustomerNPC customerNPC))
            return;

        // 손님 등록 해제
        UnregisterCustomer(customerNPC);
    }

    private void RegisterCustomer(CustomerNPC customerNPC)
    {
        /*ChairSide chairSide = customerNPC.CurrentChair.SeatSide;*/

        Debug.Log($"[Table] RegisterCustomer : {customerNPC.name}");

        ChairSide chairSide = customerNPC.CurrentChair.SeatSide;

        Debug.Log(
            $"[Table] 등록 좌석 : {chairSide}, " +
            $"Customer : {customerNPC.name}");

        // 이미 해당 좌석에 같은 손님이 등록되어 있다면 무시
        if (customerSides.TryGetValue(chairSide, out CustomerNPC currentCustomer) &&
            currentCustomer == customerNPC)
        {
            return;
        }

        customerSides[chairSide] = customerNPC;

        // 해당 좌석의 음식 목록 생성
        if (!servedItems.ContainsKey(chairSide))
        {
            servedItems[chairSide] = new List<CarrierItem>();
        }

        customerNPC.SetRestaurantID(restaurantId);

        // 식사 종료 시 테이블 위 음식 제거
        customerNPC.OnEatFinished += RemoveServedFood;
    }

    private void UnregisterCustomer(CustomerNPC customerNPC)
    {
        ChairSide chairSide = customerNPC.CurrentChair.SeatSide;

        if (!customerSides.TryGetValue(chairSide, out CustomerNPC currentCustomer))
            return;

        if (currentCustomer != customerNPC)
            return;

        // 이벤트 구독 해제
        customerNPC.OnEatFinished -= RemoveServedFood;

        // 손님 등록 해제
        customerSides.Remove(chairSide);

        // 해당 좌석의 음식 데이터 초기화
        if (servedItems.TryGetValue(chairSide, out List<CarrierItem> items))
        {
            items.Clear();
        }
    }

    // 음식 서빙
    private void ServeFood(Carrier carrier)
    {
        foreach (KeyValuePair<ChairSide, CustomerNPC> customer in customerSides)
        {
            CustomerNPC customerNPC = customer.Value;

            if (!CanServe(customerNPC))
                continue;

            ServeFoodToCustomer(customer.Key, customerNPC, carrier);
        }
    }

    // 주문 상태가 Completed가 아니고 주문 데이터가 존재하면 true
    private bool CanServe(CustomerNPC customerNPC)
    {
        if (customerNPC == null || customerNPC.MyOrder == null)
            return false;

        OrderData orderData = OrderManager.Instance.GetOrder(customerNPC.CustomerID);

        if (orderData == null)
            return false;

        return orderData.status != OrderStatus.Completed;
    }

    // 주문 데이터를 받아 요구한 수량만큼 받았는지 확인 후 아니라면 서빙함
    private void ServeFoodToCustomer(ChairSide chairSide, CustomerNPC customerNPC, Carrier carrier)
    {
        OrderData orderData = OrderManager.Instance.GetOrder(customerNPC.CustomerID);
        
        if (orderData == null)
            return;
        
        if (!servedItems.TryGetValue(chairSide, out List<CarrierItem> items))
        {
            return;
        }

        foreach (OrderItem orderItem in orderData.orderItems)
        {
            if (orderItem.IsFulfilled)
                continue;

            ServeOrderItem(chairSide, customerNPC, carrier, orderItem, items);

            // 주문이 완료되었으면 더 이상 서빙하지 않음
            if (orderData.status == OrderStatus.Completed)
                break;
        }
    }

    private void ServeOrderItem(ChairSide chairSide, CustomerNPC customerNPC, Carrier carrier, OrderItem orderItem, List<CarrierItem> items)
    {
        // 이미 필요한 수량을 모두 받은 경우
        if (orderItem.IsFulfilled)
            return;

        // 필요한 수량만큼 실제 음식을 하나씩 가져옴
        while (!orderItem.IsFulfilled)
        {
            CarrierItem item = carrier.GetOrderItem(orderItem.food.foodID);

            // Carrier에 해당 음식이 없으면 종료
            if (item == null)
                break;

            // 실제 음식 1개를 OrderManager에 서빙
            bool isServed = OrderManager.Instance.ServeFoodToTable(customerNPC.CustomerID, orderItem.food);

            // 주문 등록에 실패했다면 음식도 테이블에 놓지 않음
            if (!isServed)
                break;

            // OrderManager에 정상적으로 반영된 음식만 테이블에 배치
            PlaceFood(chairSide, item, items);
        }
    }

    private void PlaceFood(ChairSide chairSide, CarrierItem item, List<CarrierItem> items)
    {
        // 기존 음식 개수를 기준으로 음식 높이 계산
        Vector3 position = plateTransforms[(int)chairSide].position;

        position.y += items.Count * item.ItemHeight;

        item.transform.position = position;

        // 테이블 위 음식으로 등록
        items.Add(item);
    }

    private void RemoveServedFood(CustomerNPC customerNPC)
    {
        ChairSide chairSide = customerNPC.CurrentChair.SeatSide;

        if (!servedItems.TryGetValue(chairSide, out List<CarrierItem> items))
        {
            return;
        }

        // 해당 손님에게 서빙된 음식 제거
        foreach (CarrierItem item in items)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }

        items.Clear();

        // 식사 종료 이벤트 구독 해제
        customerNPC.OnEatFinished -= RemoveServedFood;
    }
}