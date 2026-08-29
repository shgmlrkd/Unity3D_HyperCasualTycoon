using UnityEngine;
public class CustomerAnimationController : MonoBehaviour
{
    private const string ISEATTING = "IsEatting";

    [SerializeField]
    private Animator animator;

    private int isStoppedHash = Animator.StringToHash("IsStopped");

    private bool isEatting = false;

    public bool IsEatting => isEatting;

    private void Awake()
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    // 걷는 애니메이션
    public void SetMoveOrSeat(bool isStopped)
    {
        animator.SetBool(isStoppedHash, isStopped);
    }

    // 먹는 애니메이션 (앉아있는 애니메이션과 먹는 애니메이션을 동시에 돌림)
    public void PlayEating()
    {
        isEatting = true;
        animator.SetBool(ISEATTING, isEatting);
    }

    // 먹는 애니메이션 멈추기
    public void StopEat()
    {
        isEatting = false;
        animator.SetBool(ISEATTING, isEatting);
    }
}
