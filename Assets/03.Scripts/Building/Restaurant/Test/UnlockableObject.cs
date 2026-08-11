using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class UnlockableObject : MonoBehaviour
{
    // 해금될 오브젝트와 연결될 발판
    [SerializeField]
    private UnlockPoint unlockPoint;

    // 해금 시 활성화 시킬지 비활성화 시킬지 인스펙터에서 설정
    [SerializeField]
    private bool activeOnUnlock = true;

    private void Awake()
    {
        unlockPoint.OnUnlocked += HandleUnlocked;
        // 해금됬을 때 활성화 <-> 비활성화 반대되야하므로 초기엔 active를 반대로함
        transform.GetChild(0).gameObject.SetActive(!activeOnUnlock);
    }

    private void OnDestroy()
    {
        unlockPoint.OnUnlocked -= HandleUnlocked;
    }

    private void HandleUnlocked()
    {
        transform.GetChild(0).gameObject.SetActive(activeOnUnlock);

        NavMeshObstacle navMeshObstacle = GetComponentInChildren<NavMeshObstacle>();
        Sequence sequence = DOTween.Sequence();

        sequence.Append(UnlockAnimation.PlayUnlockAnimation(transform))
                .OnComplete(() =>
                {
                    if (navMeshObstacle == null) return;

                    navMeshObstacle.enabled = false;
                    navMeshObstacle.enabled = true;
                });
    }
}
