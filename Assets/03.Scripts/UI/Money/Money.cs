using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
    //현재 가지고 있는 머니 text
    [SerializeField] private TextMeshProUGUI money;

    private void Awake()
    {
        //머니 표지
        money.SetText(setMoney());
    }
    //20260609
    //js.shin
    //머니 toString
    public string setMoney()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(CurrencyManager.Instance.CurrentGold);
        return sb.ToString();   
    }


    void Update()
    {
        //같으면 return
        if (money.text.Equals(setMoney())) return;
        //같지 않으면 set
        money.SetText(setMoney());
    }
}
