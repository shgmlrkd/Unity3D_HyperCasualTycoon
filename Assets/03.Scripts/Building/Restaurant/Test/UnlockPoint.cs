using System;
using UnityEngine;

public class UnlockPoint : MonoBehaviour
{
    public event Action OnUnlocked;

    private InsertBuild payMoney;

    public InsertBuild PayMoney => payMoney;
    public int BuildCost => payMoney.BuildMoney;

    private void Awake()
    {
        payMoney = GetComponentInChildren<InsertBuild>();
    }

    // 플레이어가 발판을 밟았을 때 해금이 가능한지 체크 후 가능하다면 이벤트 전달
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (payMoney.GetIsComplit())
        {
            OnUnlocked?.Invoke();
        }
    }
}