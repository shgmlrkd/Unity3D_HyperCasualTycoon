using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -7f); // 카메라 배치 위치
    [SerializeField] private Vector3 focusOffset = new Vector3(0f, 1f, 0f); // 카메라가 바라볼 캐릭터의 중심점 (예: 가슴/머리 위치)

    [Header("Follow Settings")]
    [SerializeField] private float moveSmoothSpeed = 10f;
    [SerializeField] private float rotateSmoothSpeed = 10f;

    private void LateUpdate()
    {
        if (target == null) return;

        // 1. 카메라가 실제 주시할 캐릭터 중심점 계산
        Vector3 lookAtTarget = target.position + target.TransformDirection(focusOffset);

        // 2. 캐릭터의 회전을 반영한 기본 카메라 목표 위치 계산
        Vector3 finalTargetPos = target.position + target.rotation * offset;

        // 3. 위치 보간
        transform.position = Vector3.Lerp(transform.position, finalTargetPos, Time.deltaTime * moveSmoothSpeed);

        // 4. 회전 보간 (캐릭터 중심점을 향해 대각선 아래로 내려다봄)
        Vector3 direction = lookAtTarget - transform.position;
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSmoothSpeed);
        }
    }
}