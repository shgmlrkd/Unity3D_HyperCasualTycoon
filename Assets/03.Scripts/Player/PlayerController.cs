using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(Carrier))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;

    private PlayerMovement movement;
    private PlayerAnimation animationController;
    private Carrier carrier;

    private Transform mainCameraTransform; // 메인 카메라 Transform 참조
    private bool isCarrying = false;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        animationController = GetComponent<PlayerAnimation>();
        carrier = GetComponent<Carrier>();

        // 메인 카메라 Transform 가져오기
        if (Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        if (!movement.CanMove) return;

        // 1. 이동 입력 받기
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // 2. 카메라 기준 로컬 방향으로 변환 (카메라가 바라보는 정면/우측 기준)
        Vector3 moveDirection = Vector3.zero;

        if (inputDirection.magnitude >= 0.1f)
        {
            if (mainCameraTransform != null)
            {
                // 카메라의 앞/오른쪽 벡터 가져오기 (Y축 경사는 무시하여 평면 이동 유지)
                Vector3 camForward = mainCameraTransform.forward;
                Vector3 camRight = mainCameraTransform.right;
                camForward.y = 0f;
                camRight.y = 0f;
                camForward.Normalize();
                camRight.Normalize();

                // 입력값(W/S, A/D)을 카메라 회전각에 맞춰 조합
                moveDirection = (camForward * inputDirection.z + camRight * inputDirection.x).normalized;
            }
            else
            {
                // 카메라 참조가 없으면 기존 월드 방향 백업
                moveDirection = inputDirection;
            }
        }

        // 3. 이동 스크립트 실행
        movement.Move(moveDirection, moveSpeed);

        // 4. 애니메이션 스크립트에 속도 전달
        float currentSpeed = moveDirection.magnitude >= 0.1f ? moveSpeed : 0f;
        animationController.UpdateSpeed(currentSpeed);

        // 5. 아이템 소지 여부 감지
        UpdateCarryingState();
    }

    private void UpdateCarryingState()
    {
        bool hasFood = carrier.HasItems;

        if (isCarrying != hasFood)
        {
            isCarrying = hasFood;
            animationController.SetCarrying(isCarrying);
        }
    }
}