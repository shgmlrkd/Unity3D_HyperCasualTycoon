using UnityEngine;
using UnityEngine.UI;

public class UpGradePopup : MonoBehaviour
{
    //close 버튼
    [SerializeField] private Button closePopupBtn;
    //popup
    [SerializeField] GameObject popup;
    private void Awake()
    {
        closePopupBtn.onClick.AddListener(() => onClickClosePopup());
    }
    //onClickClosePopup
    //20260609
    //js.shin
    //Close Popup
    private void onClickClosePopup()
    {
        popup.SetActive(false);
        
    }
}
