using DG.Tweening;
using UnityEngine;

public static class UnlockAnimation
{
    private const float UNLOCK_ANIMATION_DURATION = 0.5f;

    public static Tween PlayUnlockAnimation(Transform transform, bool isActive)
    {
        transform.DOKill();

        if (isActive)
        {
            transform.localScale = Vector3.zero;

            float duration = UNLOCK_ANIMATION_DURATION * 0.5f;

            return DOTween.Sequence().Append(transform.DOScaleY(1.0f, duration))
                                     .Append(transform.DOScaleX(1.0f, duration))
                                     .Join(transform.DOScaleZ(1.0f, duration))
                                     .SetEase(Ease.OutBounce);
        }

        return transform.DOScale(Vector3.zero, UNLOCK_ANIMATION_DURATION)
                        .SetEase(Ease.OutBounce);
    }
}
