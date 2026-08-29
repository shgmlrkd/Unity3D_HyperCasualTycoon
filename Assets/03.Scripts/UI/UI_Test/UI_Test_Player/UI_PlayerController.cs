using Unity.Collections;
using UnityEngine;

public class UI_PlayerController : MonoBehaviour
{
    public float moveSpeed = 0.05f;
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");


        Vector3 dir = new Vector3(h, 0.0f, v);

        dir.Normalize();
        transform.position += dir * moveSpeed*Time.deltaTime;
        //// w ->앞
        //if (Input.GetKey(KeyCode.W))
        //{
        //    transform.position += new Vector3(0.0f, 0.0f, 1.0f)* moveSpeed;
        //}
        //// s->뒤
        //if (Input.GetKey(KeyCode.S))
        //{
        //    transform.position -= new Vector3(0.0f, 0.0f, 1.0f) * moveSpeed;
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    transform.position -= new Vector3(1.0f, 0.0f, 0.0f) * moveSpeed;
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    transform.position += new Vector3(1.0f, 0.0f, 0.0f) * moveSpeed;
        //}


    }
}
