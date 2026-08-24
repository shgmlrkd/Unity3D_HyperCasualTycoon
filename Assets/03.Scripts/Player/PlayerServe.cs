using System;
using UnityEngine;

public class PlayerServe : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    private Carrier carrier;

    private TypeId typeId = TypeId.Player;

    private Table targetTable;
    private bool serveRequested;

    public bool IsMoving => playerMovement.IsMoving;

    private void OnEnable()
    {
        if (StateManager.Instance != null)
        {
            StateManager.Instance.OnUpgradeChanged += UpgradeChanged;
        }
    }

    private void Start()
    {
        int upgradeLevel = StateManager.Instance.GetPlayerUpgradeLevel();

        carrier.SetMaxCapacity(upgradeLevel);
    }

    private void OnDisable()
    {
        if (StateManager.Instance != null)
        {
            StateManager.Instance.OnUpgradeChanged -= UpgradeChanged;
        }
    }

    private void UpgradeChanged(TypeId id, int upgradeLevel)
    {
        if (typeId != id)
            return;

        carrier.SetMaxCapacity(upgradeLevel);
    }

    private void Update()
    {
        if (targetTable == null)
        {
            serveRequested = false;
            return;
        }

        if (playerMovement.IsMoving)
        {
            return;
        }

        if (!serveRequested)
            return;

        serveRequested = false;

        ServeFood();
    }

    public void SetTargetTable(Table table)
    {
        targetTable = table;
        serveRequested = true;
    }

    public void ClearTargetTable(Table table)
    {
        if (targetTable != table)
            return;

        targetTable.CancelServing();

        targetTable = null;
        serveRequested = false;
    }

    private void ServeFood()
    {
        if (targetTable == null)
            return;

        targetTable.ServeFood(carrier);
    }
}
