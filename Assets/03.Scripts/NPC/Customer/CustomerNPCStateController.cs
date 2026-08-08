using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CustomerStatePair
{
    public CustomerState state;
    public CustomerNPCState npcState;
}

public class CustomerNPCStateController : MonoBehaviour
{
    private readonly Dictionary<CustomerState, CustomerNPCState> stateDict = new Dictionary<CustomerState, CustomerNPCState>();

    [SerializeField]
    private CustomerStatePair[] statePairs;

    [SerializeField]
    private CustomerAnimationController animController;

    [SerializeField]
    private CustomerState currentCustomerState = CustomerState.None;

    private CustomerNPC npc;

    private event Action<CustomerNPCState> OnStateChanged;

    public CustomerState CurrentCustomerState => currentCustomerState;

    private void Awake()
    {
        npc = GetComponent<CustomerNPC>();

        InitState();
    }

    private void Update()
    {
        if (currentCustomerState == CustomerState.None)
            return;

        if (stateDict.TryGetValue(currentCustomerState, out CustomerNPCState state))
        {
            state.StateUpdate();
        }
    }

    private void InitState()
    {
        foreach (CustomerStatePair pair in statePairs)
        {
            if (pair.npcState == null)
                continue;

            if (!stateDict.TryAdd(pair.state, pair.npcState))
            {
                continue;
            }

            pair.npcState.Initialize(npc, animController);
        }
    }

    public void InitMoveToSeatState()
    {
        SetState(CustomerState.MoveToSeat);
    }

    public void SetState(CustomerState next)
    {
        if (currentCustomerState == next)
            return;

        // 현재 상태 종료
        if (currentCustomerState != CustomerState.None &&
            stateDict.TryGetValue(currentCustomerState, out CustomerNPCState currentState))
        {
            currentState.Exit();
        }

        // 다음 상태가 등록되어 있는지 확인
        if (!stateDict.TryGetValue(next, out CustomerNPCState nextState))
        {
            return;
        }

        currentCustomerState = next;

        nextState.Enter();

        OnStateChanged?.Invoke(nextState);
    }
}