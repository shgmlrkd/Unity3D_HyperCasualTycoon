using System.Text;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
public class InsertBuild : MonoBehaviour
{
    //빌딩 건설시 지불금액 표시
    [SerializeField] private TextMeshProUGUI buildMoneyText;
    //빌딩 가격
    [SerializeField] private int buildMoney = 0;
    //지불금액
    [SerializeField] private int payMoney = 10;
    //빌딩 가격
    //private int buildMoney = 0;
    //코루틴 start-end
    private bool isStart = true;
    //Complit
    private bool isComplit = false;
    //Complit set
    public void SetIsComplit(bool isComplit)
    {
        this.isComplit = isComplit;
    }
    //Complit get
    public bool GetIsComplit()
    {
        return isComplit;
    }

    //private void Awake()
    //{

    //    buildMoney = int.Parse(buildMoneyText.text);

    //    SetBuildMoneyText(buildMoney);
    //}
    //private void Awake()
    //{
    //    CurrencyManager.Instance.AddGold(1000);
    //    SetBuildMoney(160);
    //}

    //202600813
    //js.shin
    //SetBuildMoney : Set Build Money 
    //parp 
    //buildMoney: 빌딩 가격
    private void SetBuildMoney(int buildMoney)
    {
        //Set Build Money
        this.buildMoney = buildMoney;
        //금액 표시
        StringBuilder sb = new StringBuilder();
        sb.Append(buildMoney);
        buildMoneyText.SetText(sb.ToString());
    }
    public void DestroyInsertBuild()
    {
        //삭제
        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider collision)
    {
        //코루틴 스타드
        if (!isStart) return;


        if (!collision.CompareTag("Player")) return;
        //보유 금액이 지불 금액 보다 적으면 리턴
        //if (CurrencyManager.Instance.CurrentGold < payMoney) return;

        
        //MoneyShooter moneyShooter =
        //        collision.GetComponentInChildren<MoneyShooter>();
        //moneyShooter.SetIsStart(true);
        //moneyShooter.SetEndPoint(gameObject.transform);


        //지불 코루틴
        StartCoroutine(PayMoney());
        //if (isComplit)
        //{
        //    moneyShooter.SetIsStart(false);
        //}

    }

    //202600813
    //js.shin
    //PayMoney : Set Build Money
    private IEnumerator PayMoney()
    {

        isStart = false;
        //지불금액 마이너스
        buildMoney -= payMoney;
        //보유 금액 마이너스
        CurrencyManager.Instance.TrySpendGold(payMoney);
        yield return 10.0f;

        //Set Build Money
        SetBuildMoney(buildMoney);

        //지불완료
        if (buildMoney <= 0)
        {
            //Complit - 지불 완료
            SetIsComplit(true);
        }
        else
        {
            isStart = true;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (!collision.CompareTag("Player")) return;

        //MoneyShooter moneyShooter =
        //        collision.GetComponentInChildren<MoneyShooter>();
        //moneyShooter.SetIsStart(false);

    }
}
