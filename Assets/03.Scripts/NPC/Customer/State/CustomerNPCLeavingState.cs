using UnityEngine;

public class CustomerNPCLeavingState : CustomerNPCState
{
    public override void Enter()
    {
        // 의사 사용가능하게 바꾸는 이벤트 호출
        npc.ReleaseChair();

        CurrencyManager.Instance.AddMoney(20);


        // 다시 떠날 위치를 설정
        npc.MoveController.MoveTo(npc.LeaveTargetPos);

        // 다시 움직이는 애니메이션
        animController.SetMoveOrSeat(npc.MoveController.IsStopped());
    }

    public override void StateUpdate()
    {
        // 현재 나아가는 방향으로 회전
        RotateToMovementDirection();

        if (!npc.MoveController.IsArrived)
            return;

        npc.CompleteExit();
    }

    public override void Exit()
    {

    }
}
