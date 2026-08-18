using UnityEngine;

public class UnlockPointManager : LocalSingleton<UnlockPointManager>
{
    private UnlockPoint[] unlockPoints;

    [SerializeField]
    private int currentUnlockPointIndex = 0;

    public UnlockPoint[] UnlockPoints => unlockPoints;
    public int CurrentUnlockPointIndex => currentUnlockPointIndex;

    private void Awake()
    {
        base.Awake();

        unlockPoints = GetComponentsInChildren<UnlockPoint>(true);

        int targetIndex = 0;
        Debug.Log("SaveManager.Instance.CurrentData 까보까잉");
        if (SaveManager.Instance != null && SaveManager.Instance.CurrentData != null)
        {
            Debug.Log("SaveManager.Instance.CurrentData");
            Debug.Log(SaveManager.Instance.CurrentData);
            targetIndex = SaveManager.Instance.CurrentData.CurrentUnlockIndex;
        }

        LoadUnlockPoint(targetIndex);

        Debug.Log($"[UnlockPointManager] 해금 단계 {targetIndex} 번으로 세팅 완료!");
    }

    private void SubscribeCurrentUnlockPoint()
    {
        unlockPoints[currentUnlockPointIndex].OnUnlocked += HandleUnlockPointActivate;
    }

    private void UnsubscribeCurrentUnlockPoint()
    {
        unlockPoints[currentUnlockPointIndex].OnUnlocked -= HandleUnlockPointActivate;
    }

    private void HandleUnlockPointActivate()
    {
        UnsubscribeCurrentUnlockPoint();

        unlockPoints[currentUnlockPointIndex].gameObject.SetActive(false);

        if (currentUnlockPointIndex >= unlockPoints.Length - 1)
            return;

        currentUnlockPointIndex++;

        SaveManager.Instance?.SetDirty();

        unlockPoints[currentUnlockPointIndex].gameObject.SetActive(true);
        
        SubscribeCurrentUnlockPoint();
    }

    public void LoadUnlockPoint(int currentUnlockIndex)
    {
        if(unlockPoints == null || unlockPoints.Length == 0)
        {
            Debug.LogWarning("[UnlockPointManager] UnlockPoint가 없어요");
            return;
        }

        Debug.Log($"현재 언락 포인트 인덱스 값은! currentUnlockPointIndex : " + currentUnlockPointIndex);
        Debug.Log($"현재 언락 인덱스는! currentUnlockIndex : " + currentUnlockIndex);
        
        currentUnlockIndex = Mathf.Clamp(currentUnlockIndex, 0, unlockPoints.Length -1);

        UnsubscribeCurrentUnlockPoint();

        currentUnlockPointIndex = currentUnlockIndex;

        for (int i = 0; i < unlockPoints.Length; i++)
        {
            Debug.Log($"해금 중입니다잉 unlockPoints[" + i + "] : " + unlockPoints[i]);
            unlockPoints[i].gameObject.SetActive(i == currentUnlockIndex);
        }

        SubscribeCurrentUnlockPoint();

        Debug.Log($"[UnlockPointManager] 언락 인덱스 적용 : {CurrentUnlockPointIndex}");
    }
}