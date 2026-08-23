using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EmployeeTargetSelector : MonoBehaviour
{
    [Header("점수 세팅")]
    [SerializeField]
    private float maxWaitTime = 20.0f;

    [SerializeField]
    private float maxDistance = 10.0f;

    [Header("비율 계산")]
    [SerializeField]
    private float waitWeight = 0.7f;

    [SerializeField]
    private float distanceWeight = 0.3f;

    [SerializeField]
    private EmployeeNPC employeeNPC;

    private Table targetTable;
    private float targetScore;

    public Table TargetTable => targetTable;
    public float TargetScore => targetScore;

    public bool FindTarget()
    {
        if (employeeNPC == null)
            return false;

        targetTable = null;
        targetScore = 0.0f;

        foreach (Table table in RestaurantZoneManager.Instance.Tables)
        {
            if(!table.gameObject.activeSelf) continue;

            if (!table.NeedFood(employeeNPC.ServeFoodType))
                continue;

            foreach (CustomerNPC customer in table.Customers)
            {
                // 기다리는 시간 점수
                float waitScore = Mathf.Clamp01(customer.FoodWaitTime / maxWaitTime);

                // 각 테이블 서빙 위치와 직원 NPC의 거리
                float distance = Vector3.Distance(employeeNPC.transform.position, table.GetServePoint(employeeNPC).position);

                // 직원 NPC와의 거리 점수 (반전되야 가까울 수록 높은 점수)
                float distanceScore = 1.0f - Mathf.Clamp01(distance / maxDistance);

                // 최종 점수
                float totalScore = waitScore * waitWeight + distanceScore * distanceWeight;

                if (totalScore > targetScore)
                {
                    targetTable = table;
                    targetScore = totalScore;
                }
            }
        }

        return targetTable != null;
    }
}