using UnityEngine;

public class UnlockChecker : MonoBehaviour
{
    private RestaurantManager manager;
    private BoxCollider collider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        manager = transform.root.GetComponent<RestaurantManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 일단은 태그가 Player일 경우 된다고 가정
        if (other.CompareTag("Player"))
        {
            // 해금한 구역은 충돌체 비활성화
            if(collider.enabled)
            {
                collider.enabled = false;
            }

            // 원래라면 재화를 확인하고 지불 후 비활성화 해야함
            manager.Unlocked();
        }
    }
}