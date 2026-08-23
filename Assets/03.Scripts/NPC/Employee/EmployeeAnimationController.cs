using UnityEngine;

public class EmployeeAnimationController : MonoBehaviour
{
    private const string ISCARRY = "IsCarry";

    [SerializeField]
    private Animator animator;

    private int isStoppedHash = Animator.StringToHash("IsStopped");

    private void Awake()
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    // 걷는 애니메이션
    public void SetMove(bool isStopped)
    {
        animator.SetBool(isStoppedHash, isStopped);
    }

    // 서빙할 음식 들고있는 애니메이션
    public void SetPlayCarry(bool hasCarriedItem)
    {
        animator.SetBool(ISCARRY, hasCarriedItem);
    }
}
