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

    //- 임시 max :50
    private int upgradCountMax = 50;
    //1 count
    private int upgradCountNum = 1;

    //upgrade 처음 - 10
    private int payNum = 10;

    private void Awake()
    {   
        //논데이터 setting
        //초기 setting 값
        type.SetText("Your Manager"); //type
        img.sprite = images[0];//임시 1 데이터
        info.SetText("Your Holding Capacity");//임시 info
        upgradCount.SetText(SetUpgradCount(upgradCountMax, upgradCountNum));//임시 upgrade gauge
        PayCount.SetText(SetPayCount(payNum));//임시 UpGrade가격
        upgradeBtn.onClick.AddListener(() => onClickUpgrade());//upgrade버튼
    }
    //SetUpgradCount
    //20260609
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
    //20260609
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
    //20260609
    //js.shin
    //버튼 이벤트
    private void onClickUpgrade()
    {
        //Money Manager
        CurrencyManager.Instance.TrySpendGold(payNum);


        //레퍼런스 기준
        payNum += 10;
        upgradCountNum += 1;

        //setting upgrade count
        upgradCount.SetText(SetUpgradCount(upgradCountMax, upgradCountNum));
        //setting Pay Count
        PayCount.SetText(SetPayCount(payNum));
    }
    //임시 data setting 
    public void LoadData(string type,int imgNum, string info, int upgradCount, int payNum)
    {
        this.type.SetText(type);
        img.sprite = images[imgNum];
        this.info.SetText(info);
        this.upgradCount.SetText(SetUpgradCount(upgradCountMax, upgradCount));
        this.PayCount.SetText(SetPayCount(payNum));
        
    }
    private void Update()
    {
        //버튼 비활성화
        upgradeBtn.interactable = false;
        //현재 보유 금액 아래면 return
        if (CurrencyManager.Instance.CurrentGold < payNum) return;
        //이상이면 활성화
        upgradeBtn.interactable = true;
    }

}
