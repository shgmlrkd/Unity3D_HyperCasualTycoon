using UnityEngine;

public class CustomerNPCEatingState : CustomerNPCState
{
    private float eatingTimer;

    private const float EATING_TIME = 5.0f;

    public override void Enter()
    {
        eatingTimer = EATING_TIME;

        // 먹는 애니메이션
    }

    public override void StateUpdate()
    {
        eatingTimer -= Time.deltaTime;

        if (eatingTimer > 0.0f) return;

        npc.StateController.SetState(CustomerState.Leaving);
    }

    public override void Exit()
    {
    }
}
