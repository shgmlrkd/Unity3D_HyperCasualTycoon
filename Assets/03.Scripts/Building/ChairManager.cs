using System;
using System.Collections.Generic;
using UnityEngine;

public class ChairManager : MonoBehaviour
{
    [SerializeField] 
    private List<Chair> chairs;

    [SerializeField]
    private ChairEventData chairEventChannel;

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

        targetChair.SetState(ChairState.Occupied);

        chairEventChannel.AssignChair(customer, targetChair);
    }

    private void HandleChairReleased(Chair chair)
    {
        chair.SetState(ChairState.Available);
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
}
