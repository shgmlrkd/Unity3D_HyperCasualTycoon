using System.Text;
using TMPro;
using UnityEngine;

public class InGameMoneyPanel : MonoBehaviour
{
    //inGame - top - money text
    [SerializeField] private TextMeshProUGUI monenyTxt;

    private void Awake()
    {
        CurrencyManager.Instance.AddGold(1000);
    }

    // Update is called once per frame
    void Update()
    {
        //money text 변경시 변경
        if (int.Parse(monenyTxt.text)!= CurrencyManager.Instance.CurrentGold)
        {
            //Set Moneny Text
            monenyTxt.SetText(
                    SetMonenyTxt(CurrencyManager.Instance.CurrentGold)
             );
        }
        
    }
    //20260809
    //JS.Shin
    //SetMonenyTxt - Set Moneny Text
    //para 
    //  moneny : 보유금액
    private string SetMonenyTxt(int moneny)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(moneny);
        return sb.ToString();   
    }

}
