using System.Collections.Generic;
using UnityEngine;

public class RestaurantZoneManager : LocalSingleton<RestaurantZoneManager>
{
    private Transform[] foodPickupPoints;

    private List<Table> tables = new List<Table>();
    public IReadOnlyList<Table> Tables => tables;

    private void Awake()
    {
        base.Awake();

        KitchenZone[] zones = GetComponentsInChildren<KitchenZone>(true);
        Table[] tableArray = GetComponentsInChildren<Table>(true);

        tables.AddRange(tableArray);

        foodPickupPoints = new Transform[zones.Length];

        for (int i = 0; i < zones.Length; i++)
        {
            foodPickupPoints[i] = zones[i].transform;
        }
    }

    public Transform GetFoodPickupPoint(int index)
    {
        return foodPickupPoints[index];
    }
}
