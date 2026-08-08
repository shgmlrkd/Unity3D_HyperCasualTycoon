using TMPro;
using UnityEngine;

public class UI_Reputation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI reputationText;

    private void OnEnable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.Subscribe(EventManager.EventType.OnReputationChanged, UpdateReputationUI);
        }

        UpdateReputationUI(null);
    }

    private void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.Unsubscribe(EventManager.EventType.OnReputationChanged, UpdateReputationUI);
        }
    }

    private void UpdateReputationUI(object param)
    {
        if (reputationText != null && ReputationManager.Instance != null)
        {
            reputationText.text = $"Reputation: {ReputationManager.Instance.CurrentReputation:N0}";
        }
    }
}