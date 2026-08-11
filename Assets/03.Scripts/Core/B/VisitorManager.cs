using UnityEngine;

public class VisitorManager : MonoSingleton<VisitorManager>
{
    [Header("방문자 설정")]
    [SerializeField] private int totalVisitors = 0;
    [SerializeField] private int currentTargetIndex = 0;

    [SerializeField] private int[] festivalThresholds = new int[] { 30, 100, 500, 1000, 2500 };

    public int TotalVisitors => totalVisitors;
    public int CurrentTargetCount => (currentTargetIndex < festivalThresholds.Length)
        ? festivalThresholds[currentTargetIndex]
        : festivalThresholds[festivalThresholds.Length - 1] * 2;

    protected override void Awake()
    {
        base.Awake();
    }

    public void AddVisitor(int count = 1)
    {
        if (count <= 0) return;

        totalVisitors += count;
        Debug.Log($"[VisitorManager] 방문자 방문! 현재 총 방문자: {totalVisitors}");

        EventManager.Instance?.Publish(EventType.OnVisitorCountChanged, totalVisitors);

        CheckFestivalThreshold();
    }

    private void CheckFestivalThreshold()
    {
        int targetCount = CurrentTargetCount;

        if (totalVisitors >= targetCount)
        {
            TriggerFestival();
            currentTargetIndex++;
        }
    }

    private void TriggerFestival()
    {
        Debug.Log($"<color=yellow>[VisitorManager] 축제 발생! (달성 방문자: {totalVisitors}명)</color>");

        EventManager.Instance?.Publish(EventType.OnFestivalTriggered, currentTargetIndex);
    }

    public void SetVisitorData(int savedVisitors, int savedTargetIndex)
    {
        totalVisitors = savedVisitors;
        currentTargetIndex = savedTargetIndex;
    }
}