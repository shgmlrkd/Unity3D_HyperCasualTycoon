using System;

using UnityEngine;
using UnityEngine.UI;

public class InsertCircle : MonoBehaviour
{
    //order food 이미지
    [SerializeField] private Image foodImg;
    //1.팝업 2.음식 3.스레기통
    [SerializeField] int type;

    private void OnTriggerStay(Collider collision)
    {
       if (!collision.CompareTag("Player"))return;
        //Debug.Log("OnTriggerStay");

        CircleGauge circleGauge =
                collision.GetComponentInChildren<CircleGauge>();
        //팝업 
        if (type == 1) { }
        else if (type == 2) 
        {
            circleGauge.Test();
            //circleGauge.SetActiveGauge();
            //circleGauge.SetFoodImg(foodImg.sprite);
        }
        else if(type == 3) { }  
        

    }
    private void OnTriggerExit(Collider collision)
    {
        //if (!collision.CompareTag("Player")) return;
        //Debug.Log("OnTriggerExit");
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("OnCollisionEnter");
    //}

    //private void OnCollisionStay(Collision collision)
    //{
    //    Debug.Log("OnCollisionStay");
    //}
    //private void OnCollisionExit(Collision collision)
    //{
    //    Debug.Log("OnCollisionExit");
    //}
}
