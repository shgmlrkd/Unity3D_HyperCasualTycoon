using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ChairEventChannel", menuName = "Event/Chair Event Channel")]
public class ChairEventData : ScriptableObject
{
    public event Action<Chair> OnChairReleased;                 // 의자 사용 종료 이벤트
    public event Action<CustomerNPC> OnChairRequested;          // 손님 NPC의 의자 요청 이벤트
    public event Action<CustomerNPC, Chair> OnChairAssigned;    // 손님 NPC에게 의자 할당 이벤트

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


    public void ReleaseChair(Chair chair)
    {
        OnChairReleased?.Invoke(chair);
    }
}