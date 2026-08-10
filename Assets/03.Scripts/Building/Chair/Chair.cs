using System;
using UnityEngine;

public enum ChairState
{ 
    None = -1,

    Locked,
    Available,
    Reserved,
    Occupied,

    Length
}

public class Chair : MonoBehaviour
{
    public event Action OnStateChanged;
    public ChairState State { get; private set; } = ChairState.Locked;

    private void OnEnable()
    {
        SetState(ChairState.Available);
    }

    private void OnDisable()
    {
        SetState(ChairState.Locked);
    }

    public void SetState(ChairState state)
    {
        if (State == state)
            return;

        State = state;
        OnStateChanged?.Invoke();
    }
}
