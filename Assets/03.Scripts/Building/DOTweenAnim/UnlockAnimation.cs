using DG.Tweening;
using UnityEngine;

public static class UnlockAnimation
{
    private const float UNLOCK_ANIMATION_DURATION = 0.5f;

    public static Tween PlayUnlockAnimation(Transform transform)
    {
        transform.DOKill();
        transform.localScale = Vector3.zero;
        
        return transform.DOScale(Vector3.one, UNLOCK_ANIMATION_DURATION).SetEase(Ease.OutBounce);
    }
}
