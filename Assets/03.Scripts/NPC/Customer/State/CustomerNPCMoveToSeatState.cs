using UnityEngine;

public class CustomerNPCMoveToSeatState : CustomerNPCState
{
    public override void Enter()
    {
        npc.MoveController.MoveTo(npc.SeatTarget.position);
    }

    public override void StateUpdate()
    {
        Vector3 direction = npc.MoveController.Agent.desiredVelocity;

        if (direction.sqrMagnitude <= 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                             npc.MoveController.Agent.angularSpeed * Time.deltaTime);

        if (!npc.MoveController.IsArrived)
            return;

        npc.StateController.SetState(CustomerState.Seated);
    }

    public override void Exit()
    {
        npc.MoveController.Stop();
    }
}