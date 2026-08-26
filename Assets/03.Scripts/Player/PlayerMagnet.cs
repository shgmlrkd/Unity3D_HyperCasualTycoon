using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerMagnet : MonoBehaviour
{
    [Header("Magnet Settings")]
    [SerializeField] private float magnetRadius = 3.0f;     // 자석 끌어당김 범위
    [SerializeField] private float jumpPower = 1.5f;        // 포물선 점프 높이
    [SerializeField] private float jumpDuration = 0.5f;     // 플레이어에게 빨려 들어오는 시간
    [SerializeField] private LayerMask moneyLayer;          // Money 레이어

    [Header("Performance")]
    [SerializeField] private float scanInterval = 0.05f;    // 범위 감지 주기 (초)

    private HashSet<Money> attractingSet = new HashSet<Money>();
    private float scanTimer;

    private void Update()
    {
        scanTimer += Time.deltaTime;
        if (scanTimer >= scanInterval)
        {
            scanTimer = 0f;
            ScanForMoney();
        }
    }

    private void ScanForMoney()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, magnetRadius, moneyLayer);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].TryGetComponent<Money>(out Money money))
            {
                // 중복 방지 및 활성화 상태 확인
                if (!attractingSet.Contains(money) && money.gameObject.activeSelf)
                {
                    attractingSet.Add(money);
                    AttractWithDOTween(money);
                }
            }
        }
    }

    private void AttractWithDOTween(Money money)
    {
        // 기존 회전/드랍 트윈 정지
        money.transform.DOKill();

        // 1. 점프 흡수의 기준이 될 시작 위치 기록
        Vector3 startPos = money.transform.position;
        float elapsed = 0f;

        // 2. DOTween.To를 사용하여 플레이어가 움직여도 실시간 위치(transform.position)를 기준으로 곡선 이동
        DOTween.To(() => elapsed, x => elapsed = x, 1f, jumpDuration)
            .SetEase(Ease.InQuad)
            .OnUpdate(() =>
            {
                if (money == null || !money.gameObject.activeSelf) return;

                // 0~1 진행도에 맞춰 시작점과 플레이어의 '현재 실시간 위치' 사이를 직선 보간
                Vector3 currentTarget = transform.position;
                Vector3 basePos = Vector3.Lerp(startPos, currentTarget, elapsed);

                // y축에 점프 포물선 높이 추가 (Mathf.Sin 이용)
                float jumpOffsetY = Mathf.Sin(elapsed * Mathf.PI) * jumpPower;

                money.transform.position = basePos + new Vector3(0, jumpOffsetY, 0);
            })
            .OnComplete(() =>
            {
                if (money == null || !money.gameObject.activeSelf) return;

                // 이동 완료 시 강제로 플레이어 현재 위치로 최종 이동시켜 확실하게 Trigger 발생
                money.transform.position = transform.position;

                // 처리가 완료되었으므로 세트에서 제거 (Money.cs의 OnTriggerEnter에 의해 풀 반환됨)
                attractingSet.Remove(money);
            });
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, magnetRadius);
    }
}