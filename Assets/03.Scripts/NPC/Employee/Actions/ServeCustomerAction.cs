using UnityEngine;

public class ServeCustomerAction : IEmployeeAction
{
    public float CalculateScore(EmployeeNPC employee)
    {
        if (!employee.HasFood(employee.ServeFoodType))
            return 0.0f;

        if (!employee.TargetSelector.FindTarget())
            return 0.0f;

        return Mathf.Clamp01(employee.TargetSelector.TargetFoodWaitTime / 20.0f);
    }

    public EmployeeState GetState()
    {
        return EmployeeState.MoveToCustomer;
    }
}