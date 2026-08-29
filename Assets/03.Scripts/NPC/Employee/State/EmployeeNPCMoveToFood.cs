using UnityEngine;

public class EmployeeNPCMoveToFood : EmployeeNPCState
{
    public override void Enter()
    {
        npc.StartRestocking();
        npc.MoveController.MoveTo(npc.FoodPickupTarget);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        RotateToMovementDirection();

        if (!npc.MoveController.IsArrived)
            return;

        if (npc.IsCarryCapacityFull)
        {
            npc.CompleteRestocking();
            npc.StateController.CompleteState();
        }
    }

    public override void Exit()
    {
        npc.MoveController.Stop();
    }
}