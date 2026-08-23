using UnityEngine;

public abstract class EmployeeNPCState : MonoBehaviour
{
    protected EmployeeAnimationController animController;

    protected EmployeeNPC npc;

    public virtual void Initialize(EmployeeNPC npc, EmployeeAnimationController controller)
    {
        this.npc = npc;
        animController = controller;
    }

    public abstract void Enter();

    public virtual void StateUpdate() 
    {
        animController.SetMove(npc.MoveController.IsArrived);

        animController.SetPlayCarry(npc.HasCarriedItem);
    }

    public abstract void Exit();

    // NPC가 나아갈 방향으로 회전시키기
    protected void RotateToMovementDirection()
    {
        Vector3 direction = npc.MoveController.Agent.desiredVelocity;

        if (direction.sqrMagnitude <= 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                             npc.MoveController.Agent.angularSpeed * Time.deltaTime);
    }
}