using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Carrier carrier;
    private IInteractable currentInteractable;

    private void Awake()
    {
        carrier = GetComponent<Carrier>();
    }

    private void Update()
    {
        // 현재 감지된 영역이 있다면 지속 상호작용 실행
        if (currentInteractable != null)
        {
            currentInteractable.OnInteract(carrier);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 감지된 오브젝트에서 IInteractable 컴포넌트 탐색
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();

        // 벗어난 영역이 현재 진행 중인 영역과 같으면 감지 해제
        if (interactable == currentInteractable)
        {
            // KitchenZone 등의 타이머 리셋을 위해 캐스팅
            if (currentInteractable is KitchenZone kitchen)
            {
                kitchen.ResetTimer();
            }

            currentInteractable = null;
        }
    }
}
