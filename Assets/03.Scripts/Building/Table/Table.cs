using DG.Tweening;
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
    private Transform[] serveTransforms = new Transform[2];

    [SerializeField]
    private Transform[] plateTransforms = new Transform[2];

    [SerializeField]
    private Transform[] chairTransforms = new Transform[2];

    // 좌석 위치별 현재 손님
    private Dictionary<ChairSide, CustomerNPC> customers = new Dictionary<ChairSide, CustomerNPC>();

    // 손님별 테이블 위 음식
    private Dictionary<ChairSide, List<CarrierItem>> servedItems = new Dictionary<ChairSide, List<CarrierItem>>();
    private Sequence serveSequence;
    private bool isServing;
    
    public IEnumerable<CustomerNPC> Customers => customers.Values;

    public int RestaurantId => restaurantId;

    private void RegisterCustomer(CustomerNPC customerNPC)
    {
        ChairSide chairSide = customerNPC.CurrentChair.SeatSide;

        // 이미 해당 좌석에 같은 손님이 등록되어 있다면 무시
        if (customers.TryGetValue(chairSide, out CustomerNPC currentCustomer) &&
            currentCustomer == customerNPC)
        {
            return;
        }

        customers[chairSide] = customerNPC;

        // 해당 좌석의 음식 목록 생성
        if (!servedItems.ContainsKey(chairSide))
        {
            servedItems[chairSide] = new List<CarrierItem>();
        }

        customerNPC.SetRestaurantID(restaurantId);

        //Debug.Log($"손님 NPC가 있는 식당 : {(RestaurantType)restaurantId}");

        // 식사 종료 시 테이블 위 음식 제거
        customerNPC.OnEatFinished += RemoveServedFood;
    }

    private void UnregisterCustomer(CustomerNPC customerNPC)
    {
        ChairSide chairSide = customerNPC.CurrentChair.SeatSide;

        if (!customers.TryGetValue(chairSide, out CustomerNPC currentCustomer))
            return;

        if (currentCustomer != customerNPC)
            return;

        // 이벤트 구독 해제
        customerNPC.OnEatFinished -= RemoveServedFood;

        // 손님 등록 해제
        customers.Remove(chairSide);

        // 해당 좌석의 음식 데이터 초기화
        if (servedItems.TryGetValue(chairSide, out List<CarrierItem> items))
        {
            items.Clear();
        }
    }

    // 서빙할 위치 중 가까운 위치 반환
    public Transform GetServePoint(EmployeeNPC npc)
    {
        Transform transform = null;
        float distance = float.MaxValue;

        for (int i = 0; i < serveTransforms.Length; i++)
        {
            float servePointDistance = Vector3.Distance(npc.transform.position, serveTransforms[i].position);

            if (servePointDistance < distance)
            {
                distance = servePointDistance;
                transform = serveTransforms[i];
            }
        }

        return transform;
    }

    // 음식 서빙
    public void ServeFood(Carrier carrier)
    {
        if (isServing)
            return;

        isServing = true;

        serveSequence?.Kill();
        serveSequence = DOTween.Sequence();

        foreach (KeyValuePair<ChairSide, CustomerNPC> customer in customers)
        {
            CustomerNPC customerNPC = customer.Value;

            if (!CanServe(customerNPC))
                continue;

            AppendCustomerServeSequence(serveSequence, customer.Key, customerNPC, carrier);
        }

        serveSequence.OnComplete(() =>
        {
            isServing = false;
            serveSequence = null;
        });

        serveSequence.OnKill(() =>
        {
            isServing = false;
            serveSequence = null;
        });
    }

    // 진행 중인 서빙 취소
    public void CancelServing()
    {
        serveSequence?.Kill();
    }

    private void AppendCustomerServeSequence(Sequence sequence, ChairSide chairSide, CustomerNPC customerNPC, Carrier carrier)
    {
        if (!servedItems.TryGetValue(chairSide, out List<CarrierItem> items))
        {
            return;
        }

        OrderData orderData = OrderManager.Instance.GetOrder(customerNPC.CustomerID);

        if (orderData == null)
            return;

        foreach (OrderItem orderItem in orderData.orderItems)
        {
            if (orderItem.IsFulfilled)
                continue;

            int count = orderItem.requiredAmount;

            sequence.AppendCallback(() =>
            {
                for (int i = 0; i < count; i++)
                {
                    AppendSingleFoodServe(sequence, chairSide, customerNPC, carrier, orderItem, items);
                }
            });

            sequence.AppendInterval(0.5f);
        }
    }

    private void AppendSingleFoodServe(Sequence sequence, ChairSide chairSide, CustomerNPC customerNPC, Carrier carrier, OrderItem orderItem, List<CarrierItem> items)
    {
        if (orderItem.IsFulfilled)
            return;

        CarrierItem item = carrier.GetOrderItem(orderItem.food.foodID);

        if (item == null)
            return;

        bool isServed = OrderManager.Instance.ServeFoodToTable(customerNPC.CustomerID, orderItem.food);

        if (!isServed)
            return;

        PlaceFood(chairSide, item, items);
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

    private void PlaceFood(ChairSide chairSide, CarrierItem item, List<CarrierItem> items)
    {
        // 기존 음식 개수를 기준으로 음식 높이 계산
        Vector3 position = plateTransforms[(int)chairSide].position;

        position.y += items.Count * item.ItemHeight;

        item.transform.DOKill();

        item.transform.DOMove(position, 0.5f);

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

    public bool NeedFood(FoodType foodType)
    {
        if (customers.Count == 0)
            return false;

        foreach (CustomerNPC customer in customers.Values)
        {
            if (customer.MyOrder == null)
                continue;

            foreach (OrderItem item in customer.MyOrder.orderItems)
            {
                if (item.food.foodID == foodType && !item.IsFulfilled)
                    return true;
            }
        }

        return false;
    }

    public bool HasServedAssignedFood(FoodType foodType)
    {
        if (customers.Count == 0)
            return false;

        bool hasFood = false;

        foreach (CustomerNPC customer in customers.Values)
        {
            if (customer.MyOrder == null)
                continue;

            foreach (OrderItem item in customer.MyOrder.orderItems)
            {
                if (item.food.foodID != foodType)
                    continue;

                hasFood = true;

                if (!item.IsFulfilled)
                    return false;
            }
        }

        return hasFood;
    }

    private void OnTriggerEnter(Collider other)
    {
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
                    RegisterCustomer(customerNPC);
                    break;
                }
            }
        }

        // 테이블에 플레이어가 들어온 경우 서빙
        if (other.TryGetComponent(out PlayerServe playerServe))
        {
            playerServe.SetTargetTable(this);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent(out PlayerServe playerServe))
            return;

        if (playerServe.IsMoving)
        {
            CancelServing();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out CustomerNPC customerNPC))
            return;

        // 손님 등록 해제
        UnregisterCustomer(customerNPC);

        // 테이블에서 플레이어가 나간 경우 클리어
        if (other.TryGetComponent(out PlayerServe playerServe))
        {
            playerServe.ClearTargetTable(this);

            // 실행 중인 서빙 Sequence 종료
            serveSequence?.Kill();
            serveSequence = null;
            isServing = false;
        }
    }
}