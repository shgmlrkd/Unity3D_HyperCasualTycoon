using UnityEngine;

public class CustomerNPC : MonoBehaviour
{
    [SerializeField]
    private CustomerNPCMoveController moveController;

    [SerializeField]
    private CustomerNPCStateController stateController;

    public Transform SeatTarget { get; private set; }

    public CustomerNPCMoveController MoveController => moveController;
    public CustomerNPCStateController StateController => stateController;

    // CumstomerManager에서 풀링을 통해 생성 시 세팅
    public void Initialize(Transform seatTarget)
    {
        SeatTarget = seatTarget;
        StateController.InitMoveToSeatState();
    }
}