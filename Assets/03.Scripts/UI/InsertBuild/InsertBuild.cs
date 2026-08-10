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

    private void OnTriggerStay(Collider collision)
    {
        if (!collision.CompareTag("Player")) return;
        Debug.Log("OnBoxTriggerStay");
    }
    private void OnTriggerExit(Collider collision)
    {
        if (!collision.CompareTag("Player")) return;
        Debug.Log("OnOnBoxTriggerExit");
    }
}
