using DG.Tweening;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] private int money = 0;

    private Collider collider;

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        // 회전값 초기화, DOTween 하는 동안 충돌체 끄기
        transform.rotation = Quaternion.identity;
        collider.enabled = false;
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

    // 플레이어와 충돌 시 풀에 집어넣기
    public void ReleaseMoney()
    {
        DOTween.Kill(transform);

        PoolManager.Instance.Release(PoolType.Money, this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        CurrencyManager.Instance.AddMoney(money);

        // 플레이어가 돈을 획득
        ReleaseMoney();
    }
}
