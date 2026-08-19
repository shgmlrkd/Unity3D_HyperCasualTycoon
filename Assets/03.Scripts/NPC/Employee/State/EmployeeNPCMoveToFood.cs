using UnityEngine;

public class EmployeeNPCMoveToFood : EmployeeNPCState
{
    public override void Enter()
    {
        npc.MoveController.MoveTo(npc.FoodPickupTarget);
    }

    public override void StateUpdate()
    {
        RotateToMovementDirection();

        if(npc.IsCarryCapacityFull)
        {
            npc.StateController.CompleteState();
        }
    }

    public override void Exit()
    {
        npc.MoveController.Stop();
    }
}