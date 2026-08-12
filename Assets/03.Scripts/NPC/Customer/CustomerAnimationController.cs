using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CustomerAnimationController : MonoBehaviour
{
    private const string EATING = "Eating";
    private const string LAYER_EATING = "Eat Layer";

    [SerializeField]
    private Animator animator;

    private int layerEating;

    private int isStoppedHash = Animator.StringToHash("IsStopped");

    private bool isEatFinished = false;

    public bool IsEatFinished => isEatFinished;

    private void Awake()
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }

        layerEating = animator.GetLayerIndex(LAYER_EATING);
    }

    // 걷는 애니메이션
    public void SetMoveOrSeat(bool isStopped)
    {
        animator.SetBool(isStoppedHash, isStopped);
    }

    // 먹는 애니메이션 (앉아있는 애니메이션과 먹는 애니메이션을 동시에 돌림)
    public void PlayEating()
    {
        isEatFinished = false;
        animator.SetLayerWeight(layerEating, 1.0f);
        animator.Play(EATING, layerEating);
    }

    // 먹는 애니메이션 멈추기
    public void StopEat()
    {
        isEatFinished = true;
        animator.SetLayerWeight(layerEating, 0.0f);
    }
}
