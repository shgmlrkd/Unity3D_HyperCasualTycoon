using UnityEngine;

[CreateAssetMenu(fileName = "EmployeeData", menuName = "EmployeeNPC/EmployeeData")]
public class EmployeeData : ScriptableObject
{
    [SerializeField]
    private FoodType serveFoodType;

    [SerializeField] 
    private int maxCapacity;

    public FoodType ServeFoodType => serveFoodType;
    public int MaxCapacity => maxCapacity;
}
