using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    private readonly int speedHash = Animator.StringToHash("Speed");
    private readonly int isCarryingHash = Animator.StringToHash("IsCarrying");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // 이동 속도 업데이트
    public void UpdateSpeed(float speed)
    {
        animator.SetFloat(speedHash, speed);
    }

    // 들기 상태 업데이트
    public void SetCarrying(bool isCarrying)
    {
        animator.SetBool(isCarryingHash, isCarrying);
    }
}
