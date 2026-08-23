using UnityEngine;

public class CustomerNPCEatingState : CustomerNPCState
{
    public override void Enter()
    {
        // 먹는 애니메이션
        animController.PlayEating();
    }

    public override void StateUpdate()
    {
        if (animController.IsEatting) return;

        npc.StateController.SetState(CustomerState.Leaving);
    }

    public override void Exit()
    {
        npc.FinishEating();
    }
}
