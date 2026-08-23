using UnityEngine;

public class PlayerServe : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    private Carrier carrier;

    private Table targetTable;
    private bool serveRequested;

    public bool IsMoving => playerMovement.IsMoving;
    public Table TargetTable => targetTable;

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
