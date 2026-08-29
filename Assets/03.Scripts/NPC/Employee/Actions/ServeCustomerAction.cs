using UnityEngine;

public class ServeCustomerAction : IEmployeeAction
{
    public float CalculateScore(EmployeeNPC employee)
    {
        // 보충 중이면 서빙하지 않음
        if (employee.IsRestocking)
            return 0.0f;

        // 음식이 없으면 서빙 불가
        if (!employee.HasFood(employee.ServeFoodType))
            return 0.0f;

        // 서빙할 손님이 없으면 서빙 불가
        if (!employee.TargetSelector.FindTarget())
            return 0.0f;

        return employee.TargetSelector.TargetScore;
    }

    public EmployeeState GetState()
    {
        return EmployeeState.MoveToCustomer;
    }
}