using UnityEngine;

public class EmployeeTargetSelector : MonoBehaviour
{
    [SerializeField]
    private EmployeeNPC employeeNPC;

    private Table targetTable;
    private float targetFoodWaitTime;

    public Table TargetTable => targetTable;
    public float TargetFoodWaitTime => targetFoodWaitTime;

    public bool FindTarget()
    {
        if (employeeNPC == null)
            return false;

        targetTable = null;
        targetFoodWaitTime = -1.0f;

        foreach (Table table in RestaurantZoneManager.Instance.Tables)
        {
            if(!table.gameObject.activeSelf) continue;

            if (!table.NeedFood(employeeNPC.ServeFoodType))
                continue;

            foreach (CustomerNPC customer in table.Customers)
            {
                if (customer.FoodWaitTime > targetFoodWaitTime)
                {
                    targetFoodWaitTime = customer.FoodWaitTime;
                    targetTable = table;
                }
            }
        }

        return targetTable != null;
    }
}