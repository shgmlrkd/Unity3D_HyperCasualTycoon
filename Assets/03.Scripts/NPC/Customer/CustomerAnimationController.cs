using UnityEngine;

public class CustomerAnimationController : MonoBehaviour
{
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

    public void SetMoveOrSeat(bool isStopped)
    {
        animator.SetBool(isStoppedHash, isStopped);
    }
}
