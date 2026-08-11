using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 12.0f;

    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void Move(Vector3 direction, float speed)
    {
        Vector3 moveVelocity = Vector3.zero;

        if (direction.magnitude >= 0.1f)
        {
            // 이동 방향을 바라보도록 회전
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // 수평 이동 속도 계산
            moveVelocity = direction * speed;
        }

        // 중력 벡터 포함하여 한 번에 Move 호출
        moveVelocity.y = -9.81f;
        characterController.Move(moveVelocity * Time.deltaTime);
    }
}