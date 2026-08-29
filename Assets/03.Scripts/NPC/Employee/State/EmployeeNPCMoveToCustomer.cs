using UnityEngine;

public class EmployeeNPCMoveToCustomer : EmployeeNPCState
{
    public override void Enter()
    {
        // 서빙 대상을 새로 탐색
        if (!npc.TrySetTargetCustomer())
        {
            npc.StateController.CompleteState();
            return;
        }

        // 행동을 시작하는 시점에 다시 서빙 가능 여부 확인
        if (!npc.CanContinueServing())
        {
            npc.StateController.CompleteState();
            return;
        }

        npc.MoveController.MoveTo(npc.TargetTableServePoint.position);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        // 이동 중 플레이어가 먼저 서빙했는지 확인
        if (!npc.CanContinueServing())
        {
            npc.StateController.CompleteState();
            return;
        }

        if (!npc.MoveController.IsArrived)
        {
            RotateToMovementDirection();
            return;
        }

        // 서빙할 테이블을 바라보도록 회전
        if (!RotateToServePoint())
            return;

        // 도착 후에도 다시 검증
        if (!npc.CanContinueServing())
        {
            npc.StateController.CompleteState();
            return;
        }

        npc.ServeFood();

        if (!npc.CanContinueServing())
        {
            npc.StateController.CompleteState();
        }
    }

    public override void Exit()
    {
        npc.MoveController.Stop();
    }

    private bool RotateToServePoint()
    {
        Quaternion targetRotation = Quaternion.LookRotation(npc.TargetTableServePoint.forward);

        // NPC의 현재 회전을 목표 방향으로 회전
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 
                                                      npc.MoveController.Agent.angularSpeed * Time.deltaTime);

        float angle = Quaternion.Angle(transform.rotation, targetRotation);
        
        // 목표 방향과의 각도 차이가 1도 이하이면 회전 완료
        return angle <= 1.0f;
    }
}