public interface IInteractable
{
    // 플레이어가 영역 안에 머물 때 계속 호출되는 메서드
    void OnInteract(PlayerCarrier carrier);
}
