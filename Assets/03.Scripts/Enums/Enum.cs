public enum CustomerState
{
    None = -1,

    MoveToSeat, // 좌석으로 이동
    Seated,     // 착석 및 음식 제공 처리
    Eating,     // 식사
    Leaving,    // 퇴장

    Length
}

public enum PoolType
{
    Customer,
    Employee,
    Food
}

public enum EventType
{
    OnGameStateChanged,
        OnGoldChanged,
        OnReputationChanged,
        OnVisitorCountChanged,
        OnFestivalTriggered,
    OnMoneyChanged,
    OnOrderCreated,
    OnOrderUpdated,
    OnOrderCompleted
}

public enum GameState
{
    Init,
    Play,
    Pause,
    GameOver
}

public enum OrderStatus
{
    None,       // 주문 없음 / 초기 상태
    Waiting,    // 주문 생성 후 음식 대기 중
    Completed,  // 음식 전달 완료 및 처리 상태
    Canceled    // 손님 퇴장 등으로 인한 취소
}