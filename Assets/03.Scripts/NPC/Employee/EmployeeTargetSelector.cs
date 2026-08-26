using UnityEngine;

public class EmployeeTargetSelector : MonoBehaviour
{
    [Header("점수 세팅")]
    [SerializeField]
    private float maxWaitTime = 20.0f;

    [SerializeField]
    private float maxDistance = 10.0f;

    [Header("비율 계산")]
    [SerializeField]
    private float waitWeight = 0.5f;

    [SerializeField]
    private float distanceWeight = 0.2f;

    [SerializeField]
    private float processWeight = 0.3f;

    [SerializeField]
    private EmployeeNPC employeeNPC;

    private Table targetTable;
    private float targetScore;
    private CustomerNPC targetCustomer;
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

            // 필요한 음식 개수 (직원이 담당하는 음식)
            int needFoodCount = table.GetNeedFoodCount(employeeNPC.ServeFoodType);
            // 직원이 들고있는 음식 개수
            int carryCount = employeeNPC.CurrentCarryCount;
            // 실제로 처리할 수 있는 음식 개수
            int processCount = Mathf.Min(carryCount, needFoodCount);
            // 최대 운반량 대비 처리 가능한 음식 비율
            float processScore = processCount / (float)employeeNPC.MaxCarryCapacity;

            foreach (CustomerNPC customer in table.Customers)
            {
                // 기다리는 시간 점수
                float waitScore = Mathf.Clamp01(customer.FoodWaitTime / maxWaitTime);

                // 각 테이블 서빙 위치와 직원 NPC의 거리
                float distance = Vector3.Distance(employeeNPC.transform.position, table.GetServePoint(employeeNPC).position);

                // 직원 NPC와의 거리 점수 (반전되야 가까울 수록 높은 점수)
                float distanceScore = 1.0f - Mathf.Clamp01(distance / maxDistance);

                // 대기시간 + 거리 + 처리 효율을 반영한 최종 점수
                float totalScore = waitScore * waitWeight +
                                   distanceScore * distanceWeight +
                                   processScore * processWeight;

                Debug.Log($"[EmployeeTargetSelector] CustomerID: {customer.CustomerID} | " +
          $"Wait: {waitScore * waitWeight:F2} | " +
          $"Distance: {distanceScore * distanceWeight:F2} | " +
          $"Process: {processScore * processWeight:F2} | " +
          $"Total: {totalScore:F2}");

                if (totalScore > targetScore)
                {
                    targetTable = table;
                    targetCustomer = customer;
                    targetScore = totalScore;
                }
            }
        }

        // 최종 선택 결과 출력
        if (targetTable != null && targetCustomer != null)
        {
            Debug.Log(
                $"[EmployeeTargetSelector] 최종 타겟 | " +
                $"CustomerID: {targetCustomer.CustomerID} | " +
                $"TargetScore: {targetScore:F2}");
        }
        else
        {
            Debug.Log("[EmployeeTargetSelector] 최종 타겟 없음");
        }

        return targetTable != null;
    }
}

// 주문 받는 시간을 줄여 패널티를 주고 이 부분을 직원 NPC가 계산해서 우선도를 가지게하기