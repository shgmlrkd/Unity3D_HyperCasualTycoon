using UnityEngine;

public class CustomerNPCMoveToSeatState : CustomerNPCState
{
    public override void Enter()
    {
        npc.MoveController.MoveTo(npc.SeatTarget.position);
    }

    public override void StateUpdate()
    {
        // 현재 나아가는 방향으로 회전
        RotateToMovementDirection();

        if (!npc.MoveController.IsArrived)
            return;

        npc.StateController.SetState(CustomerState.Seated);
    }

    public override void Exit()
    {
        npc.MoveController.Stop();
    }
}