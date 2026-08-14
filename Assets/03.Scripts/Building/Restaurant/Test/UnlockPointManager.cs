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
        Initialize();
        LoadUnlockPoint(currentUnlockPointIndex);
    }

    private void Initialize()
    {
        for (int i = 0; i < unlockPoints.Length; i++)
        {
            unlockPoints[i].gameObject.SetActive(i == currentUnlockPointIndex);
        }

        SubscribeCurrentUnlockPoint();
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

        unlockPoints[currentUnlockPointIndex].gameObject.SetActive(true);
        
        SubscribeCurrentUnlockPoint();
    }

    public void LoadUnlockPoint(int currentUnlockIndex)
    {
        UnsubscribeCurrentUnlockPoint();

        currentUnlockPointIndex = currentUnlockIndex;

        for (int i = 0; i < unlockPoints.Length; i++)
        {
            unlockPoints[i].gameObject.SetActive(i == currentUnlockIndex);
        }

        SubscribeCurrentUnlockPoint();
    }
}