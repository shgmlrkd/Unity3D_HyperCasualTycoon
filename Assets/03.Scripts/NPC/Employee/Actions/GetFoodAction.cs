using UnityEngine;

public class GetFoodAction : IEmployeeAction
{
    public float CalculateScore(EmployeeNPC employee)
    {
        // 이미 보충 중이라면 계속 음식 보충
        if (employee.IsRestocking)
            return 1.0f;

        // 음식을 들고 있다면 보충할 필요 없음
        if (employee.HasFood(employee.ServeFoodType))
            return 0.0f;

        // 손이 비었으면 새로운 보충 사이클 시작
        employee.StartRestocking();

        return 1.0f;
    }

    public EmployeeState GetState()
    {
        return EmployeeState.MoveToFood;
    }
}