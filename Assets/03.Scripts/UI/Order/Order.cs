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

    private Transform camTransform;

    public Sprite FoodSprite => foodImg.sprite;

    private void Awake()
    {
        camTransform = Camera.main.transform;
    }

    private void OnEnable()
    {
        foodImg.sprite = null;
    }

    private void LateUpdate()
    {
        if (camTransform != null)
        {
            Vector3 rotation = transform.eulerAngles;

            rotation.y = camTransform.eulerAngles.y;

            transform.eulerAngles = rotation;
        }
    }

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

    // 서빙 시 갱신
    public void UpdateOrderInfo(int orderCount)
    {
        // 주문 개수가 0이면 완료된 제품
        if (orderCount == 0)
        {
            PoolManager.Instance.Release(PoolType.Order, this);
            return;
        }

        this.orderCount.SetText(SetOrderCount(orderCount));
    }

    //20260813
    //JS.Shin
    //DestroyOrder : Destroy Order
    public void DestroyOrder()
    {
        Destroy(gameObject);
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
