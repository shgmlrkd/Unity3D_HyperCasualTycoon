using System;

using UnityEngine;
using UnityEngine.UI;

public class InsertCircle : MonoBehaviour
{
    //order food 이미지
    [SerializeField] private Sprite foodImg;
    //1.팝업 2.음식 3.스레기통
    [SerializeField] int type;

    private void OnTriggerStay(Collider collision)
    {
        
        if (!collision.CompareTag("Player") && !collision.CompareTag("EmployeeNPC")) return;
        //play, npc 자식 - circle Gauge 
        CircleGauge circleGauge =
                collision.GetComponentInChildren<CircleGauge>(true);
        //팝업 
        if (type == 1) 
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
                //팝업 open
                popup.OpenPopup();

            //circleGauge 비활성화
            circleGauge.SetActiveGauge(false);
        }
        //음식
        else if (type == 2) 
        {
            if (foodImg != null)
            {
                //circleGauge 내의 푸드 이미지
                circleGauge.SetFoodImg(foodImg);
            }
            //circleGauge 활성화
            circleGauge.SetActiveGauge(true);   
        }
        //쓰레기
        else if (type == 3) 
        {
            //npc x
            if (collision.CompareTag("EmployeeNPC")) return;
            //circleGauge 활성화
            circleGauge.SetActiveGauge(true);
        }  
        

    }

    private void OnTriggerExit(Collider collision)
    {
        
        if (!collision.CompareTag("Player") && !collision.CompareTag("EmployeeNPC")) return;
        //play, npc 자식 - circle Gauge 
        CircleGauge circleGauge =
                collision.GetComponentInChildren<CircleGauge>(true);
        //팝업 
        if (type == 1)
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
