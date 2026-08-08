using TMPro;
using UnityEngine;

public class UI_Currency : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;

    private void OnEnable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.Subscribe(EventManager.EventType.OnGoldChanged, UpdateGoldUI);
        }

        UpdateGoldUI(null);
    }

    private void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.Unsubscribe(EventManager.EventType.OnGoldChanged, UpdateGoldUI);
        }
    }

    private void UpdateGoldUI(object param)
    {
        if (goldText != null && CurrencyManager.Instance != null)
        {
            goldText.text = $"{CurrencyManager.Instance.CurrentGold:N0} Gold";
        }
    }
}