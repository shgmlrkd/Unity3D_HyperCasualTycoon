using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ChairEventChannel", menuName = "Event/Chair Event Channel")]
public class ChairEventData : ScriptableObject
{
    // Customer -> ChairManager
    public event Action<CustomerNPC> OnChairRequested;          // 손님 NPC의 의자 요청 이벤트

    // ChairManager -> Customer
    public event Action<CustomerNPC, Chair> OnChairAssigned;    // 손님 NPC에게 의자 할당 이벤트

    // Customer -> ChairManager
    public event Action<Chair> OnChairReleased;                 // 의자 사용 종료 이벤트

    // ChairManager -> CustomerManager
    public event Action<bool> OnChairAvailabilityChanged;       // 사용할 수 있는 의자가 있을 때 알려주는 이벤트
  
    // 손님 NPC의 의자 할당 요청 이벤트 발생
    public void RequestChair(CustomerNPC customer)
    {
        OnChairRequested?.Invoke(customer);
    }

    // 손님NPC가 목표로 할 의자 전달 이벤트
    public void AssignChair(CustomerNPC customer, Chair chair)
    {
        OnChairAssigned?.Invoke(customer, chair);
    }

    // 의자 상태 초기화하는 이벤트
    public void ReleaseChair(Chair chair)
    {
        OnChairReleased?.Invoke(chair);
    }

    // 사용할 의자가 있는지 알려주는 이벤트
    public void NotifyChairAvailability(bool isAvailable)
    {
        OnChairAvailabilityChanged?.Invoke(isAvailable);
    }     
}