using Restaurant.Orders;
using System;
using UnityEngine;

public class CustomerNPC : MonoBehaviour
{
    [SerializeField]
    private CustomerNPCMoveController moveController;

    [SerializeField]
    private CustomerNPCStateController stateController;

    [SerializeField]
    private ChairEventData chairEventChannel;

    private Chair currentChair;

    private OrderData myOrder = null;

    private int customerID = -1;
    private int restaurantID = -1;
    private bool isEatFinished = false;

    public Chair CurrentChair => currentChair;
    public Vector3 LeaveTargetPos { get; private set; }
    public Transform SeatTarget { get; private set; }
    public OrderData MyOrder => myOrder;
    public CustomerNPCMoveController MoveController => moveController;
    public CustomerNPCStateController StateController => stateController;

    public event Action<CustomerNPC> OnEatFinished;
    public event Action<CustomerNPC> OnExitCompleted;
    public int CustomerID => customerID;
    public int RestaurantID => restaurantID;

    private void OnEnable()
    {
        chairEventChannel.OnChairAssigned += HandleChairAssigned;
    }

    private void OnDisable()
    {
        chairEventChannel.OnChairAssigned -= HandleChairAssigned;
    }

    // 이벤트를 통해 자신에게 할당된 의자를 전달받음 
    private void HandleChairAssigned(CustomerNPC customer, Chair chair)
    {
        if (customer != this)
            return;

        Initialize(chair);
    }

    // 목표 의자, 의자 위치, 떠날 위치, 손님 NPC의 상태를 초기화
    private void Initialize(Chair targetChair)
    {
        myOrder = null;
        restaurantID = -1;
        isEatFinished = false;
        LeaveTargetPos = transform.position;

        currentChair = targetChair;
        SeatTarget = targetChair.transform;

        StateController.InitMoveToSeatState();
    }

    // 목적지로 삼은 의자 해제하는 이벤트
    public void ReleaseChair()
    {
        chairEventChannel.ReleaseChair(currentChair);
    }

    // 손님 NPC가 다시 풀로 돌아갈 때 호출하는 이벤트
    public void CompleteExit()
    {
        OnExitCompleted?.Invoke(this);
    }

    // 손님NPC 고유ID 세팅
    public void AssignCustomerID(int customerID)
    {
        if (this.customerID != -1) return;
        
        this.customerID = customerID;
    }

    // 현재 들어간 레스토랑 ID 세팅
    public void SetRestaurantID(int index)
    {
        restaurantID = index;
    }

    public void SetMyOrder(OrderData orderData)
    {
        myOrder = orderData;
    }

    public void FinishEating()
    {
        isEatFinished = true;

        OnEatFinished?.Invoke(this);
    }
}