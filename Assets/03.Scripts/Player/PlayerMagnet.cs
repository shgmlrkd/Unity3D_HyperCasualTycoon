using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerMagnet : MonoBehaviour
{
    [Header("Magnet Settings")]
    [SerializeField] private float magnetRadius = 3.0f;     // 자석 끌어당김 범위
    [SerializeField] private LayerMask moneyLayer;          // Money 레이어

    [Header("Performance")]
    [SerializeField] private float scanInterval = 0.05f;    // 범위 감지 주기 (초)
    [SerializeField] private int maxMoneyCount = 100;

    private Collider[] overlapResults;
    private float scanTimer;
    private void Awake()
    {
        overlapResults = new Collider[maxMoneyCount];
    }

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
        int count = Physics.OverlapSphereNonAlloc(transform.position, magnetRadius, overlapResults, moneyLayer);

        for (int i = 0; i < count; i++)
        {
            if (!overlapResults[i].TryGetComponent(out Money money))
                continue;

            if (money.IsAttracted)
                continue;

            money.AttractTo(transform);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, magnetRadius);
    }
}