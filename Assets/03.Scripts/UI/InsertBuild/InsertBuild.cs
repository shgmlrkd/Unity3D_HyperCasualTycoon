using System.Text;
using TMPro;
using UnityEngine;
using System.Collections;

public class InsertBuild : MonoBehaviour
{
    //빌딩 건설시 지불금액 표시
    [SerializeField] private TextMeshProUGUI buildMoneyText;
    //빌딩 가격
    [SerializeField] private int buildMoney = 0;
    //지불금액
    [SerializeField] private int payMoney = 10;
    //코루틴 - 지불 지연 시간
    [SerializeField] private float payWaitTime = 0.05f;

    private const float WAIT_TIME = 1.0f;

    //빌딩 가격
    //private int buildMoney = 0;
    //코루틴 start-end
    private Coroutine payMoneyCoroutine;
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
    private void Awake()
    {
        SetBuildMoney(buildMoney);
    }

    //202600813
    //js.shin
    //SetBuildMoney : Set Build Money 
    //para 
    //buildMoney: 빌딩 가격
    public void SetBuildMoney(int buildMoney)
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
        //보유 금액이 지불 금액 보다 적으면 리턴
        if (CurrencyManager.Instance.CurrentMoney < payMoney) return;

        if (!collision.TryGetComponent(out PlayerMovement player)) return;

        // 플레이어가 움직이면 지불 중인 코루틴 중단
        if (player.IsMoving)
        {
            if (payMoneyCoroutine != null)
            {
                StopCoroutine(payMoneyCoroutine);
                payMoneyCoroutine = null;
            }

            return;
        }

        // 이미 지불 중이면 다시 시작하지 않음
        if (payMoneyCoroutine != null) return;

        payMoneyCoroutine = StartCoroutine(PayMoney(collision));
    }

    //202600813
    //js.shin
    //PayMoney : Set Build Money
    private IEnumerator PayMoney(Collider collision)
    {
        // 지불 시작 전 유예시간
        yield return new WaitForSeconds(WAIT_TIME);

        MoneyShooter moneyShooter = collision.GetComponentInChildren<MoneyShooter>();

        while (buildMoney > 0)
        {
            // 실제 지불할 금액 계산
            int currentPayMoney = Mathf.Min(payMoney, buildMoney);

            // 지불 모션
            moneyShooter.ShootMoneny(transform);

            // 건설에 필요한 금액에서 실제 지불 금액만 차감
            buildMoney -= currentPayMoney;

            // 보유 금액 차감
            CurrencyManager.Instance.TrySpendMoney(currentPayMoney);

            // 건설 금액 갱신
            SetBuildMoney(buildMoney);
            yield return new WaitForSeconds(payWaitTime);

        }

        // 지불 완료
        SetIsComplit(true);
        payMoneyCoroutine = null;
    }
    private void OnTriggerExit(Collider collision)
    {
        if (!collision.CompareTag("Player")) return;

    }
}
