using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerCarrier))] // PlayerCarrier 필수 컴포넌트 추가
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;

    private PlayerMovement movement;
    private PlayerAnimation animationController;
    private PlayerCarrier carrier; // 추가

    private bool isCarrying = false;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        animationController = GetComponent<PlayerAnimation>();
        carrier = GetComponent<PlayerCarrier>(); // 추가
    }

    private void Update()
    {
        // 1. 이동 입력 받기
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // 2. 이동 스크립트 실행
        movement.Move(moveDirection, moveSpeed);

        // 3. 애니메이션 스크립트에 속도 전달
        float currentSpeed = moveDirection.magnitude >= 0.1f ? moveSpeed : 0f;
        animationController.UpdateSpeed(currentSpeed);

        // 4. 아이템 소지 여부 감지 (자동들기 애니메이션)
        UpdateCarryingState();
    }

    private void UpdateCarryingState()
    {
        // Carrier에 아이템(피자/햄버거)이 하나라도 있는지 확인
        bool hasFood = carrier.HasItems;

        // 상태가 달라졌을 때만 애니메이션 파라미터 업데이트 (불필요한 호출 방지)
        if (isCarrying != hasFood)
        {
            isCarrying = hasFood;
            animationController.SetCarrying(isCarrying);
        }
    }
}
