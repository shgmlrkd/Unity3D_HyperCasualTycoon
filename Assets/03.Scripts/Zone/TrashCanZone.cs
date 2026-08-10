using UnityEngine;

public class TrashCanZone : MonoBehaviour, IInteractable
{
    [Header("Trash Settings")]
    [SerializeField] private float discardInterval = 0.2f; // 아이템 버리는 간격 (초)

    private float timer = 0f;

    public void OnInteract(Carrier carrier)
    {
        // 들고 있는 아이템이 없으면 작동 안 함
        if (!carrier.HasItems) return;

        timer += Time.deltaTime;

        if (timer >= discardInterval)
        {
            timer = 0f;

            //한 번에 모두 버리기
            carrier.ClearAllItems();
        }
    }

    public void ResetTimer()
    {
        timer = 0f;
    }
}