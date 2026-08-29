using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpGradePopup : MonoBehaviour
{
   
    //close 버튼
    [SerializeField] private Button closePopupBtn;
    //private List<ContentsData> contentsData
    //[SerializeField] private List<ContentsData> contentList;
    [SerializeField] private ContentsListSOData contentsList;
    //[SerializeField] private List<ContentsData> contentList;


    ContentsScrollView contentsScrollView;

    //팝업 상태 
    //원상태에서 닫기 해도 열리는거 방지
    public bool OpenState {  get;  set; }    

    private void Awake()
    {
        DOTween.Init();
        // transform 의 scale 값을 모두 0.1f로 변경합니다.
        transform.localScale = Vector3.one * 0.1f;
        // 객체를 비활성화 합니다.
        gameObject.SetActive(false);
        //close button AddListener
        closePopupBtn.onClick.AddListener(() => onClickClosePopup());
        OpenState = false;
        //contents Scroll View
        contentsScrollView = gameObject.GetComponentInChildren<ContentsScrollView>(true);

    }


   
    //20260812
    //js.shin
    //OpenPopup : Open Popup   
    //para 
    //popupType : SO Data 순서(확정성 관련 생성)
    public void OpenPopup(RestaurantType restaurantType)
    {
        //show popup
        ShowPopup();
        //set content data
        //contentsScrollView.SetContentDate(contentList[popupType]);
        foreach (var contents in contentsList.ContentsList)
        {
            if (contents.type == restaurantType) {
                contentsScrollView.SetRestaurantType(restaurantType);
                contentsScrollView.SetContentDate(contents.contents);
            }
        }
        

        //contents 생성
        contentsScrollView.CreateScrollItems();
    }

    //20260812
    //js.shin
    //ShowPopup : Show Popup  
    private void ShowPopup()
    {
        gameObject.SetActive(true);
        // DOTween 함수를 차례대로 수행하게 해줍니다.
        var seq = DOTween.Sequence();

        // DOScale 의 첫 번째 파라미터는 목표 Scale 값, 두 번째는 시간입니다.
        seq.Append(transform.DOScale(1.1f, 0.2f));
        seq.Append(transform.DOScale(1f, 0.1f));
        seq.Play().OnComplete(() =>
        {
            OpenState = true;
        });
    }

    //onClickClosePopup
    //20260609
    //js.shin
    //Close Popup
    public void onClickClosePopup()
    {
        SoundManager.Instance.PlaySFX(SoundType.ButtonClick);
        var seq = DOTween.Sequence();

        transform.localScale = Vector3.one * 0.2f;

        seq.Append(transform.DOScale(1.1f, 0.1f));
        seq.Append(transform.DOScale(0.2f, 0.2f));

        // OnComplete 는 seq 에 설정한 애니메이션의 플레이가 완료되면
        // { } 안에 있는 코드가 수행된다는 의미입니다.
        // 여기서는 닫기 애니메이션이 완료된 후 객첼르 비활성화 합니다.
        seq.Play().OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
