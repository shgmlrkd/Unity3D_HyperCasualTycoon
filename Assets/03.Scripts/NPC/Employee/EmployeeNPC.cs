using System;
using UnityEngine;

public class EmployeeNPC : MonoBehaviour
{
    [SerializeField]
    private EmployeeTargetSelector targetSelector;

    [SerializeField]
    private EmployeeNPCStateController stateController;

    [SerializeField]
    private NPCMoveController moveController;

    /*[SerializeField]
    private int index = 0;

    [SerializeField]*/
    private FoodType serveFoodType = FoodType.None;
    private TypeId employeeTypeId;
    [SerializeField]
    private Carrier carrier;

    private Transform foodTargetTransform;

    private Table targetTable;

    // 음식을 다시 채우기 시작했는지 여부
    private bool isRestocking;
    public EmployeeTargetSelector TargetSelector => targetSelector;
    public EmployeeNPCStateController StateController => stateController;
    public NPCMoveController MoveController => moveController;
    
    public Transform TargetTableServePoint
    {
        get
        {
            if (targetTable == null)
                return transform;

            return targetTable.GetServePoint(this);
        }
    }
    public Vector3 FoodPickupTarget => foodTargetTransform.position;
    public FoodType ServeFoodType => serveFoodType;
    public int CurrentCarryCount => carrier.CurrentCount;
    public int MaxCarryCapacity => carrier.MaxCapacity;
    public bool IsRestocking => isRestocking;
    public bool IsCarryCapacityFull => carrier.IsFull;
    public bool HasCarriedItem => carrier.CurrentCount > 0;

    private void Start()
    {
        //SetEmployee(index);
    }

    private void OnEnable()
    {
        if (StateManager.Instance != null)
        {
            StateManager.Instance.OnUpgradeChanged += UpgradeChanged;
        }
    }

    private void OnDisable()
    {
        if (StateManager.Instance != null)
        {
            StateManager.Instance.OnUpgradeChanged -= UpgradeChanged;
        }
    }

    public void SetEmployee(int index, TypeId typeId)
    {
        serveFoodType = (FoodType)index;
        employeeTypeId = typeId;
        foodTargetTransform = RestaurantZoneManager.Instance.GetFoodPickupPoint(index);
    }

    private void UpgradeChanged(TypeId id, int upgradeLevel)
    {
        if (employeeTypeId != id)
            return;

        carrier.SetMaxCapacity(upgradeLevel);
    }

    public bool TrySetTargetCustomer()
    {
        // 이전 대상 제거
        targetTable = null;

        if (!targetSelector.FindTarget())
            return false;

        targetTable = targetSelector.TargetTable;

        return targetTable != null;
    }

    public bool CanContinueServing()
    {
        if (targetTable == null)
            return false;

        // 해당 음식에 아직 서빙할 주문이 있는지 확인
        if (!targetTable.NeedFood(ServeFoodType))
            return false;

        // 직원이 해당 음식을 가지고 있는지 확인
        if (!carrier.HasFood(ServeFoodType))
            return false;

        return true;
    }

    public void ServeFood()
    {
        if (targetTable == null)
            return;

        targetTable.ServeFood(carrier);
    }

    public bool HasFood(FoodType serveFoodType)
    {
        return carrier.HasFood(serveFoodType);
    }

    public void StartRestocking()
    {
        isRestocking = true;
    }

    public void CompleteRestocking()
    {
        isRestocking = false;
    }
}