using Restaurant.Orders;
using UnityEngine;

public class CustomerNPCSeatedState : CustomerNPCState
{
    public override void Enter()
    {
        npc.ResetFoodWaitTime();

        animController.SetMoveOrSeat(npc.MoveController.IsStopped());
        transform.rotation = Quaternion.LookRotation(npc.CurrentChair.transform.forward);

        // 착석 처리
        // 주문/음식 제공 요청

        if (OrderGenerator.Instance != null)
        {
            OrderData orderData = OrderGenerator.Instance.CreateRandomOrder(npc.CustomerID, (RestaurantType)npc.RestaurantID);

            // 주문 후 UI를 띄워야함
            OrderUIManager.Instance.SetCustomerOrderUI(orderData, transform);

            npc.SetMyOrder(orderData);
        }
    }

    public override void StateUpdate()
    {
        if (npc.MyOrder == null)
            return;

        if (npc.MyOrder.status == OrderStatus.Completed)
        {
            npc.StateController.SetState(CustomerState.Eating);
        }

        npc.AddFoodWaitTime(Time.deltaTime);
    }

    public override void Exit() {}
}
