using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CustomerNPCMoveController : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;

    public NavMeshAgent Agent => agent;

    public bool IsArrived
    {
        get
        {
            if (!agent.hasPath)
                return false;

            if (agent.pathPending)
                return false;

            if (agent.remainingDistance > agent.stoppingDistance)
                return false;

            return true;
        }
    }

    private void Awake()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        agent.updateRotation = false;
    }

    public void MoveTo(Vector3 targetPosition)
    {
        agent.isStopped = false;
        agent.SetDestination(targetPosition);
    }

    public void Stop()
    {
        agent.ResetPath();

        agent.isStopped = true;
    }

    public bool IsStopped()
    {
        return agent.isStopped;
    }
}