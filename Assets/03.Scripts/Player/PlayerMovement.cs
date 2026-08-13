using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 12.0f;

    [Header("Map Boundary Settings")]
    [SerializeField] private float minX = -20f;
    [SerializeField] private float maxX = 20f;
    [SerializeField] private float minZ = -20f;
    [SerializeField] private float maxZ = 20f;

    [SerializeField] private CameraFollow cameraFollow;
    
    private CharacterController characterController;

    private bool canMove = true;
    public bool CanMove => canMove;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        cameraFollow.OnStoppedPlayerMove += SetMove;
    }

    private void SetMove(bool canMove)
    {
        this.canMove = canMove;
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
    private void LateUpdate()
    {
        // 이동 계산이 끝난 후 플레이어 위치를 min ~ max 사이로 제한
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, minZ, maxZ);

        transform.position = clampedPosition;
    }
}