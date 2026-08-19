using System.Collections.Generic;
using UnityEngine;

public class EmployeeDecision : MonoBehaviour
{
    private List<IEmployeeAction> actions;

    private void Awake()
    {
        actions = new List<IEmployeeAction>
        {
            new GetFoodAction(),
            new ServeCustomerAction()
        };
    }

    // 현재 어떤 행동을 할지 계산해 다음 행동을 선택함
    public IEmployeeAction SelectAction(EmployeeNPC employee)
    {
        IEmployeeAction bestAction = null;
        float bestScore = -1.0f;

        foreach (IEmployeeAction action in actions)
        {
            float score = action.CalculateScore(employee);

            if (score > bestScore)
            {
                bestScore = score;
                bestAction = action;
            }
        }

        return bestAction;
    }
}
