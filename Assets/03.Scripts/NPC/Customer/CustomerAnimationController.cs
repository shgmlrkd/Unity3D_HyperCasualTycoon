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

    public void SetMoveOrSeat(bool isStopped)
    {
        animator.SetBool(isStoppedHash, isStopped);
    }

    public void PlayEating()
    {
        isEatFinished = false;
        animator.SetLayerWeight(layerEating, 1.0f);
        animator.Play(EATING, layerEating);
    }

    public void StopEat()
    {
        isEatFinished = true;
        animator.SetLayerWeight(layerEating, 0.0f);
    }
}
