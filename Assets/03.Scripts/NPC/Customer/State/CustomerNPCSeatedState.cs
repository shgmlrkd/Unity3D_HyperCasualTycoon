using UnityEngine;

public class CustomerNPCSeatedState : CustomerNPCState
{
    private const float ORDER_TIME = 3.0f;
    private float orderTimer;

    /*
    private bool isFoorServed = false;
    private OrderData myOrder = null;

     [Header("돈 뭉치 프리팹")]
    [SerializeField] private GameObject moneyChunkPrefab;
     */

    public override void Enter()
    {
        
        /*
         isFoodServed = false;
         */
        
        orderTimer = 0.0f;
        animController.SetMoveOrSeat(npc.MoveController.IsStopped());
        transform.rotation = Quaternion.LookRotation(npc.CurrentChair.transform.forward);

        // 착석 처리
        // 주문/음식 제공 요청
        
        /*
          
         희강님, 
         의자 InstanceID를 기반으로 OrderManager에 주문 등록한다면,
        int chairID = npc.CurrentChair.GetInstanceID(); 뭐 이런 식으로 NPC한테 위치(테이블이든 의자든 암튼 고유 데이터 받고
        RestaurantType restaurantType = (RestaurantType)npc.RestaurantID; 이런 식으로 레스토랑 타입도 받고

        if(OrderGenerator.Instance != null)
        {
            myOrder = OrderGenerator.Instance.CreateRandomOrder(chairID, restaurantType); 뭐 이런 식으로 주문하면 될 것 같아요
        }

         */
    }

    /*
     * 
     그리고 주문 완료 관련을 여기서 해결한다면
    private void OnOrderCompletedHandler(OrderData orderData)
    {
        if(orderData == myOrder && myOrder != null)
        {
            isFoodServed = true;
        }
    }
    이런 식으로 하면 주문 완료 처리를 해도 되구요.
     */

    public override void StateUpdate()
    {
        // 음식 제공 이벤트를 받으면 Eating으로 전환 <- 해야할것
        /*
         이것도
        if(isFoodServed)
        {
            npc.StateController.SetState(CustomerState.Eating); 이렇게 해도 되구요.
        }
         */
        // 임시로 일정 시간 지나면 Eating 상태로 변환

        orderTimer += Time.deltaTime;

        if(orderTimer > ORDER_TIME)
        {
            npc.StateController.SetState(CustomerState.Eating);
        }
    }

    public override void Exit()
    {
        /*
         얘도,
        EventManager.Instance?.Unsubscribe(EventType.OnOrderCompleted, OnOrderCompletedHandler);

        if (myOrder != null && moneyChunkPrefab != null)
        {
            int chunkCount = myOrder.GetTotalMoneyDropCount(); // 음식 개수 x 4 덩어리

            for (int i = 0; i < chunkCount; i++)
            {
                Vector3 randomOffset = new Vector3(
                    Random.Range(-0.6f, 0.6f),
                    0.1f,
                    Random.Range(-0.6f, 0.6f)
                );

                Vector3 spawnPos = transform.position + randomOffset;
                Object.Instantiate(moneyChunkPrefab, spawnPos, Quaternion.identity);
            }
        }
        뭐 대충 이런 식으로 하면 식사 끝나고 돈 뿌리고 나가는 걸로 하면 되지 않을까 싶습니다.
         */
    }
}
