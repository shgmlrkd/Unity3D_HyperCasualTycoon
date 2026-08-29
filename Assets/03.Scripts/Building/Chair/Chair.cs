using System;
using UnityEngine;

public class Chair : MonoBehaviour
{
    [SerializeField]
    private ChairSide seatSide;

    public event Action OnStateChanged;
    public ChairSide SeatSide => seatSide;
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
