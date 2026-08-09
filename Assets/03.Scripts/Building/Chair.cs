using UnityEngine;

public enum ChairState
{ 
    None = -1,

    Available,
    Reserved,
    Occupied,

    Length
}

public class Chair : MonoBehaviour
{
    public ChairState State { get; private set; } = ChairState.Available;

    public void SetState(ChairState state)
    {
        State = state;
    }
}
