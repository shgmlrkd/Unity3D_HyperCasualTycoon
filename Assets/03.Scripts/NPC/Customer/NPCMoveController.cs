using UnityEngine;
using UnityEngine.AI;

public class NPCMoveController : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;

    public NavMeshAgent Agent => agent;

    public bool IsArrived
    {
        get
        {
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

    // 목적지로 이동 시키기
    public void MoveTo(Vector3 targetPosition)
    {
        agent.isStopped = false;
        agent.SetDestination(targetPosition);
    }

    // 목적지 제거 후 멈추기
    public void Stop()
    {
        agent.ResetPath();

        agent.isStopped = true;
    }

    // 풀에서 나올 때 리셋 시키는 메서드
    public void ResetAgent(Vector3 position)
    {
        agent.isStopped = true;
        agent.ResetPath();
        agent.velocity = Vector3.zero;
        agent.Warp(position);
    }

    // 멈춰있는지 확인하는 메서드
    public bool IsStopped()
    {
        return agent.isStopped;
    }
}