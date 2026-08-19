using UnityEngine;

public class EmployeeNPCMoveToCustomer : EmployeeNPCState
{
    public override void Enter()
    {
        npc.SetTargetCustomer();
        npc.MoveController.MoveTo(npc.TargetTablePosition);
    }

    public override void StateUpdate()
    {
        RotateToMovementDirection();

        if (!npc.MoveController.IsArrived)
            return;

        npc.ServeFood();

        if (npc.IsServeComplete())
        {
            npc.StateController.CompleteState();
        }
    }

    public override void Exit()
    {
        npc.MoveController.Stop();
    }
}
