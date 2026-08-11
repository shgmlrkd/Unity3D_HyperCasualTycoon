using System;
using UnityEngine;

public class UnlockPoint : MonoBehaviour
{
    public event Action OnUnlocked;

    private bool isUnlocked = false;

    // 플레이어가 발판을 밟았을 때 해금이 가능한지 체크 후 가능하다면 이벤트 전달
    private void OnTriggerEnter(Collider other)
    {
        if (isUnlocked) return;

        if (!other.CompareTag("Player")) return;

        isUnlocked = true;
        OnUnlocked?.Invoke();
    }
}