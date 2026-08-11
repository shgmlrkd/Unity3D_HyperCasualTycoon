using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target; // 따라갈 플레이어

    [Header("Follow Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -7f); // 카메라와 플레이어 간의 거리
    [SerializeField] private float smoothSpeed = 5.0f; // 카메라 이동 부드러움 정도

    private void LateUpdate()
    {
        if (target == null) return;

        // 플레이어 위치 + 오프셋 위치 계산
        Vector3 desiredPosition = target.position + offset;

        // 부드러운 카메라 이동 (Lerp)
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;

        // 카메라는 항상 플레이어를 바라보도록 설정
        transform.LookAt(target);
    }
}