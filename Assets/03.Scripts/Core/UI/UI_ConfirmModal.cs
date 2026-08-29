using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ConfirmModal : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private TextMeshProUGUI yesButtonText;
    [SerializeField] private TextMeshProUGUI noButtonText;

    private Action onYesCallback;
    private Action onNoCallback;

    private void Awake()
    {
        if (yesButton != null)
        {
            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(OnClickYes);
        }

        if (noButton != null)
        {
            noButton.onClick.RemoveAllListeners();
            noButton.onClick.AddListener(OnClickNo);
        }
    }

    public void ShowConfirm(string message, Action onYes, Action onNo = null, string yesText = "네", string noText = "아니요")
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        if (messageText != null) messageText.text = message;
        if (yesButtonText != null) yesButtonText.text = yesText;
        if (noButtonText != null) noButtonText.text = noText;

        if (noButton != null) noButton.gameObject.SetActive(true);

        onYesCallback = onYes;
        onNoCallback = onNo;
    }

    public void ShowAlert(string message, Action onConfirm = null, string confirmText = "확인")
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        if (messageText != null) messageText.text = message;
        if (yesButtonText != null) yesButtonText.text = confirmText;

        if (noButton != null) noButton.gameObject.SetActive(false);

        onYesCallback = onConfirm;
        onNoCallback = null;
    }

    private void OnClickYes()
    {
        SoundManager.Instance.PlaySFX(SoundType.ButtonClick);
        gameObject.SetActive(false);
        onYesCallback?.Invoke();
    }

    private void OnClickNo()
    {
        SoundManager.Instance.PlaySFX(SoundType.ButtonClick);
        gameObject.SetActive(false);
        onNoCallback?.Invoke();
    }
}