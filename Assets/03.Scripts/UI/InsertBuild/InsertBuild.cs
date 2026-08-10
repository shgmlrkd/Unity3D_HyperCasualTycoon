using System.Text;
using TMPro;
using UnityEngine;

public class InsertBuild : MonoBehaviour
{
    //빌딩 건설시 지불금액
    [SerializeField] private TextMeshProUGUI money;

    private void SetBuildMoney(int pay)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(pay);
        money.SetText(sb.ToString());
    }
}
