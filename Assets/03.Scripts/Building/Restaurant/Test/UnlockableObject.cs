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

    private GameObject targetObject;

    public UnlockPoint UnlockPoint => unlockPoint;

    private void Awake()
    {
        targetObject = transform.GetChild(0).gameObject;

        unlockPoint.OnUnlocked += HandleUnlocked;
        // 해금됬을 때 활성화 <-> 비활성화 반대되야하므로 초기엔 active를 반대로함
        targetObject.SetActive(!activeOnUnlock);
    }

    private void OnDestroy()
    {
        if (unlockPoint == null) return;

        unlockPoint.OnUnlocked -= HandleUnlocked;
    }

    private void HandleUnlocked()
    {
        NavMeshObstacle navMeshObstacle = GetComponentInChildren<NavMeshObstacle>(true);

        if (activeOnUnlock)
        {
            targetObject.SetActive(true);

            Tween tween = UnlockAnimation.PlayUnlockAnimation(transform, true);

            tween.OnComplete(() =>
            {
                ResetNavMeshObstacle(navMeshObstacle);
            });

            return;
        }

        Tween disableTween = UnlockAnimation.PlayUnlockAnimation(transform, false);

        disableTween.OnComplete(() =>
        {
            ResetNavMeshObstacle(navMeshObstacle);
            targetObject.SetActive(false);
        });
    }

    private void ResetNavMeshObstacle(NavMeshObstacle navMeshObstacle)
    {
        if (navMeshObstacle == null) return;

        navMeshObstacle.enabled = false;
        navMeshObstacle.enabled = true;
    }

    public void UnlockObject()
    {
        NavMeshObstacle navMeshObstacle = GetComponentInChildren<NavMeshObstacle>(true);

        if (activeOnUnlock)
        {
            targetObject.SetActive(true);
            ResetNavMeshObstacle(navMeshObstacle); 
        }
        else
        {
            targetObject.SetActive(false);
        }
    }
}