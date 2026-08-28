using DG.Tweening;
using System;
using UnityEngine;

public class MoneyObj : MonoBehaviour
{
    private float moveDuration = 0.25f;
    private float jumpPower = 0.8f;

    //도착 위치
    private Transform endPoint;
    private Tween moveTween;

    //202600814
    //js.shin
    //SetEndPoint : EndPoint Set
    //para
    //endPoint : 도착 위치
    public void SetEndPoint(Transform endPoint)
    {
        this.endPoint = endPoint;

        // 돈 내는 연출
        StartMove();
    }

    // 점프해서 이동 후 파괴
    private void StartMove()
    {
        moveTween?.Kill();

        moveTween = transform.DOJump(endPoint.position, jumpPower, 1, moveDuration)
                             .SetEase(Ease.OutQuad)
                             .OnComplete(OnMoveComplete);
    }

    private void OnMoveComplete()
    {
        SoundManager.Instance.PlaySFX(SoundType.Money);

        moveTween = null;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        moveTween?.Kill();
    }
}
