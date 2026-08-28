using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Content : MonoBehaviour
{
    //타입 - player, NPC
    [SerializeField] private TextMeshProUGUI type;
    //이미지
    [SerializeField] private Image img;
    //이미지 prite 배열
    [SerializeField] public Sprite[] images;
    //팝업 정보
    [SerializeField] private TextMeshProUGUI info;
    //upgrade gauge
    [SerializeField] private TextMeshProUGUI upgradCount;
    //UpGrade가격
    [SerializeField] private TextMeshProUGUI PayCount;
    //Upgrade 버튼
    [SerializeField] private Button upgradeBtn;

    //content data
    private ContentData contentData;
    //restaurant Type
    private RestaurantType restaurantType;
    //upgrade 가격
    private int UpgradePay = 0;

    //private Image background;



    private void Awake()
    {
        //background = GetComponent<Image>();
        upgradeBtn.onClick.AddListener(() => onClickUpgrade());//upgrade버튼
    }
    //SetUpgradCount
    //20260809
    //js.shin
    //머니 toString
    //para : 
    //      upgradCountMax : 최고점 upgrade
    //      upgradCountNum : 현재 upgradCountNum
    private string SetUpgradCount(int upgradCountMax, int upgradCountNum)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(upgradCountNum);
        sb.Append("/");
        sb.Append(upgradCountMax);
        return sb.ToString();
    }
    //SetPayCount
    //20260809
    //js.shin
    //머니 toString
    //para : 
    //      payNum : 현재 payNum
    private string SetPayCount(int payNum)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("$ ");
        sb.Append(payNum);
        return sb.ToString();
    }
    //onClickUpgrade
    //20260809
    //js.shin
    //버튼 이벤트
    private void onClickUpgrade()
    {
        SoundManager.Instance.PlaySFX(SoundType.ButtonClick);

        //Money Manager
        //CurrencyManager.Instance.TrySpendGold(UpgradePay);
        CurrencyManager.Instance.TrySpendMoney(UpgradePay);
        //Upgrade
        StateManager.Instance.AddUpgradeLevel(restaurantType, contentData.TypeId, 1);
        //Load Data
        LoadData();


    }
    public void SetRestaurantType(RestaurantType restaurantType)
    {
        this.restaurantType = restaurantType;   
    }

    //202600812
    //js.shin
    //SetDate : content data
    //Para :
    //contentData : content data
    public void SetDate(ContentData contentData)
    {
        //data set
        this.contentData = contentData;
        //Load Data
        LoadData();
    }


    //202600812
    //js.shin
    //LoadData : Load Data
    private void LoadData()
    {
        //content data set
        type.SetText(contentData.TypeText);
        img.sprite = contentData.Image;
        info.SetText(contentData.Info);
        
        //level 표시
        upgradCount.SetText(
            SetUpgradCount(contentData.UpgradeMaxCount,
                           StateManager.Instance.GetPopupUpgradeLevel(restaurantType, contentData.TypeId))
            );
        //NPC 초기값 level = 0, pay  = 500
        if(StateManager.Instance.GetPopupUpgradeLevel(restaurantType, contentData.TypeId) == 0)
        {
            UpgradePay = 500;
        }
        else
        {
            UpgradePay = StateManager.Instance.GetPopupUpgradeLevel(restaurantType, contentData.TypeId) * 10;
        }
        PayCount.SetText(
            SetPayCount(UpgradePay)
            );

        //content 색 변경
        type.color = contentData.TextColor;
        info.color = contentData.TextColor;
        upgradCount.color = contentData.TextColor;
        PayCount.color = Color.white;

        GetComponent<Image>().color = contentData.BackgroundColor;
        //background.color = contentData.BackgroundColor;
    }
    private void Update()
    {
        //버튼 비활성화
        upgradeBtn.interactable = false;
        //현재 보유 금액 아래면 return
        if (CurrencyManager.Instance.CurrentMoney < UpgradePay
            //최대치 업그레이드
            || contentData.UpgradeMaxCount <= StateManager.Instance.GetPopupUpgradeLevel(restaurantType, contentData.TypeId)) return;

        //이상이면 활성화
        upgradeBtn.interactable = true;
    }

}
