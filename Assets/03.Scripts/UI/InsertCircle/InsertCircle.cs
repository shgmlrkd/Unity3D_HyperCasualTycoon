using System;
using UnityEngine;

public class InsertCircle : MonoBehaviour
{

    
    private void OnTriggerStay(Collider collision)
    {
       if (!collision.CompareTag("Player"))return;
        Debug.Log("OnTriggerStay");
    }
    private void OnTriggerExit(Collider collision)
    {
        if (!collision.CompareTag("Player")) return;
        Debug.Log("OnTriggerExit");
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
