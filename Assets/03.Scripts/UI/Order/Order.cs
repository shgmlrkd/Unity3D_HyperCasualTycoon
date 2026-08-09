using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Order : MonoBehaviour
{
    //order food 이미지
    [SerializeField] private Image foodImg;
    //order Count
    [SerializeField] private TextMeshProUGUI orderCount;



    //20260809
    //JS.Shin
    //SetOrderInfo - 주문정보 셋팅
    //para 
    //  image : 푸드 이미지
    //  orderCount : 주문 count
    public void SetOrderInfo(Sprite image, int orderCount)
    {
        //푸드이미지
        foodImg.sprite = image; 
        //주문 count
        this.orderCount.SetText(SetOrderCount(orderCount));    
    }

    //20260809
    //JS.Shin
    //SetOrderCount - 주문 Count string 셋팅
    //para 
    //  orderCount : 주문 count
    //retrun
    //  string : 주문 수
    private string SetOrderCount(int orderCount)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("X ");
        sb.Append(orderCount);
        return sb.ToString();
    }
}
