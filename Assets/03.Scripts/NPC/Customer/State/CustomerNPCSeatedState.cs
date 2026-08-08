using UnityEngine;

public class CustomerNPCSeatedState : CustomerNPCState
{
    public override void Enter()
    {
        animController.SetMoveOrSeat(npc.MoveController.IsStopped());

        // 착석 처리
        // 주문/음식 제공 요청
    }

    public override void StateUpdate()
    {
        // 음식 제공 이벤트를 받으면 Eating으로 전환
    }

    public override void Exit()
    {

    }
}
