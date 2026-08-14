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

    Pizza,
    Hambuger,
    Cake,
    IceCream,

    Length
}

public enum CircleType
{
    None = -1,
    Basic,
    Food,
    Popop
}
public enum PopupType
{
    None = -1,
    MainRestaurant = 0
}