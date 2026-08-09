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