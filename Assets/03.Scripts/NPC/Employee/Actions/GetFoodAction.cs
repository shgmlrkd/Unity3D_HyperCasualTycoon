using UnityEngine;

public class GetFoodAction : IEmployeeAction
{
    public float CalculateScore(EmployeeNPC employee)
    {
        if (employee.IsCarryCapacityFull)
            return 0.0f;

        if (!employee.TargetSelector.FindTarget())
            return 0.0f;

        float fillRate = employee.CurrentCarryCount / (float)employee.MaxCarryCapacity;

        return 1.0f - fillRate;
    }

    public EmployeeState GetState()
    {
        return EmployeeState.MoveToFood;
    }
}