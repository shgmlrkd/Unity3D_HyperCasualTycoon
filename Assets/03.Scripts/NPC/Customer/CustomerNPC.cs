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

    private int restaurantID = -1;

    public Chair CurrentChair => currentChair;
    public Vector3 LeaveTargetPos { get; private set; }
    public Transform SeatTarget { get; private set; }
    public CustomerNPCMoveController MoveController => moveController;
    public CustomerNPCStateController StateController => stateController;

    public int RestaurantID => restaurantID;

    public event Action<CustomerNPC> OnExitCompleted;
    
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
        currentChair = targetChair;
        LeaveTargetPos = transform.position;
        SeatTarget = targetChair.transform;
        StateController.InitMoveToSeatState();
    }

    public void ReleaseChair()
    {
        chairEventChannel.ReleaseChair(currentChair);
    }

    public void CompleteExit()
    {
        OnExitCompleted?.Invoke(this);
    }

    public void SetRestaurantID(int index)
    {
        restaurantID = index;
    }

    public void ResetRestaurantID()
    {
        restaurantID = -1;
    }
}