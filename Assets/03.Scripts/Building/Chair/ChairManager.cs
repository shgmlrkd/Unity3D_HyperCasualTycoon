using System;
using UnityEngine;

public class ChairManager : MonoBehaviour
{
    [SerializeField]
    private ChairEventData chairEventChannel;

    private Chair[] chairs;

    private void Awake()
    {
        chairs = GetComponentsInChildren<Chair>(true);

        foreach (Chair chair in chairs)
        {
            chair.OnStateChanged += RefreshChairAvailability;
        }
    }

    private void OnDestroy()
    {
        if (chairs == null) return;

        foreach (Chair chair in chairs)
        {
            chair.OnStateChanged -= RefreshChairAvailability;
        }
    }

    private void OnEnable()
    {
        chairEventChannel.OnChairRequested += HandleChairRequested;
        chairEventChannel.OnChairReleased += HandleChairReleased;
    }

    private void OnDisable()
    {
        chairEventChannel.OnChairRequested -= HandleChairRequested;
        chairEventChannel.OnChairReleased -= HandleChairReleased;
    }

    private void HandleChairRequested(CustomerNPC customer)
    {
        if (!TryGetChair(customer.transform.position, out Chair targetChair))
            return;

        chairEventChannel.AssignChair(customer, targetChair);

        RefreshChairAvailability();
    }

    private void HandleChairReleased(Chair chair)
    {
        chair.SetState(ChairState.Available);

        RefreshChairAvailability();
    }

    // 이용할 수 있는 의자를 찾음
    private bool TryGetChair(Vector3 npcPosition, out Chair chair)
    {
        chair = null;

        float closestDistance = float.MaxValue;

        foreach (Chair currentChair in chairs)
        {
            if (currentChair.State != ChairState.Available)
                continue;

            float distance = Vector3.Distance(npcPosition, currentChair.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                chair = currentChair;
            }
        }

        if (chair == null)
            return false;

        chair.SetState(ChairState.Reserved);

        return true;
    }

    private void RefreshChairAvailability()
    {
        bool hasAvailableChair = HasAvailableChair();

        chairEventChannel.NotifyChairAvailability(hasAvailableChair);
    }

    private bool HasAvailableChair()
    {
        foreach (Chair chair in chairs)
        {
            if (chair.State == ChairState.Available)
                return true;
        }

        return false;
    }
}
