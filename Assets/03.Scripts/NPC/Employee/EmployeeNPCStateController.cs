using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EmployeeStatePair
{
    public EmployeeState state;
    public EmployeeNPCState npcState;
}

public class EmployeeNPCStateController : MonoBehaviour
{
    private readonly Dictionary<EmployeeState, EmployeeNPCState> stateDict = new Dictionary<EmployeeState, EmployeeNPCState>();

    [SerializeField]
    private EmployeeStatePair[] statePairs;

    [SerializeField]
    private EmployeeAnimationController animController;
    
    [Header("Decision Settings")]
    [SerializeField]
    private float decisionDelay = 1.0f;

    [SerializeField]
    private EmployeeState currentState = EmployeeState.None;

    private EmployeeNPC npc;

    private EmployeeDecision decision;

    private float decisionTimer;

    public EmployeeState CurrentEmployeeState => currentState;

    private void Awake()
    {
        npc = GetComponent<EmployeeNPC>();
        decision = GetComponent<EmployeeDecision>();

        InitState();
    }

    private void InitState()
    {
        foreach (EmployeeStatePair pair in statePairs)
        {
            if (pair.npcState == null)
                continue;

            if (!stateDict.TryAdd(pair.state, pair.npcState))
                continue;

            pair.npcState.Initialize(npc, animController);
        }
    }

    private void Update()
    {
        // 일정 시간마다 어떤 행동을 할지 결정
        decisionTimer += Time.deltaTime;

        if (decisionTimer >= decisionDelay)
        {
            decisionTimer = 0.0f;
            EvaluateNextAction();
        }

        if (stateDict.TryGetValue(currentState, out EmployeeNPCState state))
        {
            state.StateUpdate();
        }
    }

    // 지금 해야할 행동을 결정하고 그에 맞는 상태로 변경
    private void EvaluateNextAction()
    {
        IEmployeeAction action = decision.SelectAction(npc);

        if (action == null)
            return;

        SetState(action.GetState()); 
    }

    private void SetState(EmployeeState state)
    {
        if (currentState == state)
            return;
        
        if (stateDict.TryGetValue(currentState, out EmployeeNPCState curstate))
        {
            curstate.Exit();
        }

        // 다음 상태가 등록되어 있는지 확인
        if (!stateDict.TryGetValue(state, out EmployeeNPCState nextState))
        {
            return;
        }

        currentState = state;

        nextState.Enter();
    }

    public void CompleteState()
    {
        if (currentState == EmployeeState.None)
            return;

        if(stateDict.TryGetValue(currentState, out EmployeeNPCState state))
        {
            state.Exit();
        }

        currentState = EmployeeState.None;
        decisionTimer = 0.0f;
    }
}
