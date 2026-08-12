using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{ 
    [SerializeField]
    private int restaurantId;

    private Dictionary<ChairSide, CustomerNPC> customerSides = new Dictionary<ChairSide, CustomerNPC>();

    public IReadOnlyDictionary<ChairSide, CustomerNPC> CustomerSides => customerSides;
    public int RestaurantId => restaurantId;

    private void OnTriggerEnter(Collider other)
    {
        // 테이블과 손님 NPC가 충돌 시 자리가 왼쪽인지 오른쪽인지 확인해서 넣음
        // 현재 레스토랑의 ID를 손님 NPC에게 전달함
        if(other.TryGetComponent(out CustomerNPC customerNPC)) 
        {
            customerSides[customerNPC.CurrentChair.SeatSide] = customerNPC;
            customerNPC.SetRestaurantID(restaurantId);
        }
    }
}