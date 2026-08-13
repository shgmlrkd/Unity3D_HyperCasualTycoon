using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    private UnlockPoint[] unlockPoints;

    private int currentIndex = 0;

    private void Awake()
    {
        unlockPoints = GetComponentsInChildren<UnlockPoint>(true);
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 1; i < unlockPoints.Length; i++)
        {
            unlockPoints[i].gameObject.SetActive(false);
        }

        SubscribeCurrentUnlockPoint();
    }

    private void SubscribeCurrentUnlockPoint()
    {
        unlockPoints[currentIndex].OnUnlocked += HandleUnlockPointActivate;
    }

    private void HandleUnlockPointActivate()
    {
        unlockPoints[currentIndex].OnUnlocked -= HandleUnlockPointActivate;

        unlockPoints[currentIndex].gameObject.SetActive(false);

        currentIndex++;

        if (currentIndex >= unlockPoints.Length)
            return;

        unlockPoints[currentIndex].gameObject.SetActive(true);
        
        SubscribeCurrentUnlockPoint();
    }
}