using UnityEngine;

public abstract class CustomerNPCState : MonoBehaviour
{
    protected CustomerAnimationController animController;

    protected CustomerNPC npc;

    public virtual void Initialize(CustomerNPC npc, CustomerAnimationController controller)
    {
        this.npc = npc;
        animController = controller;
    }

    public abstract void Enter();

    public virtual void StateUpdate() { }

    public abstract void Exit();
}