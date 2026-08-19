using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class EmployeeNPC : MonoBehaviour
{
    [SerializeField]
    private EmployeeTargetSelector targetSelector;

    [SerializeField]
    private EmployeeNPCStateController stateController;

    [SerializeField]
    private NPCMoveController moveController;

    [SerializeField]
    private FoodType serveFoodType = FoodType.None;

    [SerializeField]
    private Carrier carrier;

    private Transform foodTargetTransform;

    private Table targetTable; 

    public EmployeeTargetSelector TargetSelector => targetSelector;
    public EmployeeNPCStateController StateController => stateController;
    public NPCMoveController MoveController => moveController;
    
    public Vector3 TargetTablePosition
    {
        get
        {
            if (targetTable == null)
                return transform.position;

            return targetTable.ServePoint.position;
        }
    }
    public Vector3 FoodPickupTarget => foodTargetTransform.position;
    public FoodType ServeFoodType => serveFoodType;
    public bool IsCarryCapacityFull => carrier.IsFull;
    public int CurrentCarryCount => carrier.CurrentCount;
    public int MaxCarryCapacity => carrier.MaxCapacity;

    private void Start()
    {
        SetEmployee(0);
    }

    public void SetEmployee(int index)
    {
        serveFoodType = (FoodType)index;
        foodTargetTransform = RestaurantZoneManager.Instance.GetFoodPickupPoint(index);
    }

    public void SetTargetCustomer()
    {
        if (!targetSelector.FindTarget())
            return;

        targetTable = targetSelector.TargetTable;
    }

    public bool IsServeComplete()
    {
        if (targetTable == null)
            return true;

        if (!targetTable.NeedFood(ServeFoodType))
            return true;

        return !carrier.HasFood(ServeFoodType);
    }

    public void ServeFood()
    {
        targetTable.ServeFood(carrier);
    }

    public bool HasFood(FoodType serveFoodType)
    {
        return carrier.HasFood(serveFoodType);
    }
}