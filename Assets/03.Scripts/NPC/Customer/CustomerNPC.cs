using Restaurant.Orders;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomerNPC : MonoBehaviour, IPoolInitialize
{
    [SerializeField]
    private NPCMoveController moveController;

    [SerializeField]
    private CustomerNPCStateController stateController;

    [SerializeField]
    private ChairEventData chairEventChannel;

    [SerializeField]
    private SkinnedMeshRenderer skinnedMeshRenderer;

    private Chair currentChair;

    private OrderData myOrder = null;

    private float foodWaitTime = 0.0f;
    private int customerID = -1;
    private int restaurantID = -1;
    private bool isEatFinished = false;

    public Chair CurrentChair => currentChair;
    public Vector3 LeaveTargetPos { get; private set; }
    public Transform SeatTarget { get; private set; }
    public OrderData MyOrder => myOrder;
    public NPCMoveController MoveController => moveController;
    public CustomerNPCStateController StateController => stateController;

    public event Action<CustomerNPC> OnEatFinished;
    public event Action<CustomerNPC> OnExitCompleted;
    public float FoodWaitTime => foodWaitTime;
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

    public void SetSkinnedMesh(Mesh mesh)
    {
        skinnedMeshRenderer.sharedMesh = mesh;
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

    // 현재 들어간 레스토랑 ID 세팅
    public void SetRestaurantID(int index)
    {
        restaurantID = index;
    }

    // 손님NPC 고유ID 세팅
    public void InitializePool(int id)
    {
        customerID = id;
    }

    public void SetMyOrder(OrderData orderData)
    {
        myOrder = orderData;
    }

    public void ResetFoodWaitTime()
    {
        foodWaitTime = 0.0f;
    }

    public void AddFoodWaitTime(float deltaTime)
    {
        foodWaitTime += deltaTime;
    }

    public bool NeedFood(FoodType foodType)
    {
        if (myOrder == null)
            return false;

        List<OrderItem> orders = myOrder.orderItems;

        for(int i = 0; i < orders.Count; i++)
        {
            if (orders[i].food.foodID == foodType &&
                !orders[i].IsFulfilled)
                return true;
        }

        return false;
    }

    public void FinishEating()
    {
        isEatFinished = true;

        OnEatFinished?.Invoke(this);
    }
}