using System;

using UnityEngine;
using UnityEngine.UI;

public class InsertCircle : MonoBehaviour
{
    //order food 이미지
    [SerializeField] private Sprite foodImg;
    [Header("Circle Type")]
    //1.Popop 2.Food 3.Basic
    [SerializeField] private CircleType type;

    [Header("Popup Type")]
    //파업 타입(확정성 관련해서 생성)
    //MainRestaurant : 0
    [SerializeField] private RestaurantType restaurantType;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Carrier carrier)) return;

        //play, npc 자식 - circle Gauge 
        CircleGauge circleGauge =
                other.GetComponentInChildren<CircleGauge>(true);

        if (CircleType.Food == type)
        {
            // 음식 용량 최대치면 상호작용 X
            if (carrier.IsFull)
            {
                return;
            }

            if (foodImg != null)
            {
                //circleGauge 내의 푸드 이미지
                circleGauge.SetFoodImg(foodImg);
            }

            //circleGauge 활성화
            circleGauge.SetActiveGauge(true);
        }
        //쓰레기
        else if (CircleType.Basic == type)
        {
            //npc x
            if (other.CompareTag("EmployeeNPC")) return;

            // 음식을 들고 있지 않으면 X
            if (!carrier.HasItems) return;

            //circleGauge 활성화
            circleGauge.SetActiveGauge(true);
        }
    }

    private void OnTriggerStay(Collider collision)
    {

        if (!collision.TryGetComponent(out Carrier carrier)) return;

        //play, npc 자식 - circle Gauge 
        CircleGauge circleGauge =
                collision.GetComponentInChildren<CircleGauge>(true);
        
        //팝업 
        if (CircleType.Popup == type) 
        {

            //npc x
            if (collision.CompareTag("EmployeeNPC")) return;
            //circleGauge 활성화
            circleGauge.SetActiveGauge(true);

            //circleGauge Complit
            if (!circleGauge.Complit) return;
            
            //팝업
            UpGradePopup popup = 
                GameObject.FindWithTag("UI").GetComponentInChildren<UpGradePopup>(true);

            
            
            //팝업 상태 비활성화
            //원상태에서 닫기 해도 열리는거 방지
            if (!popup.OpenState)
            {
                //오픈 팝업 - restaurant Type
                popup.OpenPopup(restaurantType);
            }
                

            //circleGauge 비활성화
            circleGauge.SetActiveGauge(false);
        }
        //음식
        else if (CircleType.Food == type) 
        {
            // 최대치면 게이지 비활성화
            if (carrier.IsFull)
            {
                circleGauge.SetActiveGauge(false);
                return;
            }
        }
        //쓰레기
        else if (CircleType.Basic == type) 
        {
            //npc x
            if (collision.CompareTag("EmployeeNPC")) return;
            
            // 음식을 들고 있지 않으면 X
            if (!carrier.HasItems)
            {
                circleGauge.SetActiveGauge(false);
                return;
            }
        }  
        

    }

    private void OnTriggerExit(Collider collision)
    {
        
        if (!collision.CompareTag("Player") && !collision.CompareTag("EmployeeNPC")) return;
        //play, npc 자식 - circle Gauge 
        CircleGauge circleGauge =
                collision.GetComponentInChildren<CircleGauge>(true);
        //팝업 
        if (CircleType.Popup == type)
        {
            //npc x
            if (collision.CompareTag("EmployeeNPC")) return;
            //팝업
            UpGradePopup popup = 
                GameObject.FindWithTag("UI").GetComponentInChildren<UpGradePopup>(true);
            //팝업 상태 비활성화
            //원상태에서 닫기 해도 열리는거 방지
            popup.OpenState = false;
        }

        //order 푸드 이미지 초기화
        circleGauge.SetFoodImg(null);
        //Complit 초기화
        circleGauge.Complit = false;
        //타임, 게이지 초기와
        circleGauge.SetResetData();
        //circleGauge 비활성화
        circleGauge.SetActiveGauge(false);
    }
}