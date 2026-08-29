using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Money : MonoBehaviour
{
    [SerializeField] private int money = 0;
    [SerializeField]
    private float jumpPower = 1.0f;
    [SerializeField]
    private float jumpDuration = 0.2f;

    private Collider collider;

    private Tween attractTween;

    private bool isAttracted = false;

    public bool IsAttracted => isAttracted;

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        ResetMoney();
    }

    private void OnDisable()
    {
        KillTweens();

        isAttracted = false;
    }

    // 스폰 후 회전 시키기
    public void Spawn(Vector3 position)
    {
        transform.position = position;

        transform.DOKill();

        transform.DORotate(new Vector3(0.0f, 360.0f, 0.0f), 1.0f, RotateMode.FastBeyond360)
                 .SetLoops(-1, LoopType.Incremental);
    }

    // 드랍 후 충돌체 켜서 플레이어가 trigger로 상호작용할 수 있게하기
    public void Drop(Vector3 targetPosition)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOJump(targetPosition, 1.5f, 1, 0.4f))
                .OnComplete(() =>
                {
                    collider.enabled = true;
                });
    }

    private void ResetMoney()
    {
        KillTweens();

        transform.rotation = Quaternion.identity;

        collider.enabled = false;

        isAttracted = false;
    }

    private void KillTweens()
    {
        transform.DOKill();

        attractTween?.Kill();
        attractTween = null;
    }

    // 플레이어와 충돌 시 풀에 집어넣기
    private void ReleaseMoney()
    {
        KillTweens();

        PoolManager.Instance.Release(PoolType.Money, this);
    }

    public void AttractTo(Transform target)
    {
        if (isAttracted || target == null)
            return;

        isAttracted = true;

        collider.enabled = false;

        // 기존 회전/드랍 트윈 정지
        KillTweens();

        // 1. 점프 흡수의 기준이 될 시작 위치 기록
        Vector3 startPos = transform.position;
        float elapsed = 0f;

        attractTween = DOTween.To(() => elapsed, value => elapsed = value, 1f, jumpDuration)
            .SetEase(Ease.InQuad)
            .OnUpdate(() =>
            {
                if (!gameObject.activeSelf)
                    return;

                if (target == null)
                    return;

                Vector3 position = Vector3.Lerp(startPos, target.position, elapsed);

                position.y += Mathf.Sin(elapsed * Mathf.PI) * jumpPower;

                transform.position = position;
            })
            .OnComplete(() =>
            {
                attractTween = null;

                if (!gameObject.activeSelf) return;

                transform.position = target.position;

                CurrencyManager.Instance.AddMoney(money);

                SoundManager.Instance.PlaySFX(SoundType.Money);

                ReleaseMoney();
            });
    }
}
