public enum CustomerState
{
    None = -1,

    MoveToSeat, // 좌석으로 이동
    Seated,     // 착석 및 음식 제공 처리
    Eating,     // 식사
    Leaving,    // 퇴장

    Length
}

public enum EmployeeState
{
    None = -1,

    MoveToFood,         // 음식받으러 이동 후 음식 받기
    MoveToCustomer,     // 손님 NPC에게 이동 후 서빙

    Length
}

public enum PoolType
{
    Customer,
    Order,
    Money
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

public enum ChairState
{
    None = -1,

    Locked,     // 잠김
    Available,  // 사용 가능
    Reserved,   // 예약됨
    Occupied,   // 사용 중

    Length
}

public enum ChairSide
{
    Front,   // 의자 위치 (앞)
    Back     // 의자 위치  (뒤)
}

public enum RestaurantType
{
    None = -1,

    PizzaHamburger,
    CakeIcecream,

    Length
}

public enum FoodType
{
    None = -1,

    Pizza,      // 피자            (직원 NPC)
    Hambuger,   // 햄버거          (직원 NPC)
    Cake,       // 케이크          (직원 NPC)
    IceCream,   // 아이스크림       (직원 NPC)
    All,        // 플레이어는 모든 음식 다 가능

    Length
}

public enum CircleType
{
    None = -1,
    Basic,
    Food,
    Popup
}
public enum TypeId
{
    None = -1,
    Player,
    Employee01,
    Employee02,
    Employee03,
    Employee04,
    Length
}

public enum SoundType
{
    ButtonClick,
    Food,
    Money
}