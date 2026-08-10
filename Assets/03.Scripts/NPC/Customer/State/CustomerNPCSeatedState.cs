using UnityEngine;

public class CustomerNPCSeatedState : CustomerNPCState
{
    private const float ORDER_TIME = 3.0f;
    private float orderTimer;

    public override void Enter()
    {
        orderTimer = 0.0f;
        animController.SetMoveOrSeat(npc.MoveController.IsStopped());
        transform.rotation = Quaternion.LookRotation(npc.CurrentChair.transform.forward);
        // 착석 처리
        // 주문/음식 제공 요청
    }

    public override void StateUpdate()
    {
        // 음식 제공 이벤트를 받으면 Eating으로 전환 <- 해야할것

        // 임시로 일정 시간 지나면 Eating 상태로 변환

        orderTimer += Time.deltaTime;

        if(orderTimer > ORDER_TIME)
        {
            npc.StateController.SetState(CustomerState.Eating);
        }
    }

    public override void Exit()
    {

    }
}
