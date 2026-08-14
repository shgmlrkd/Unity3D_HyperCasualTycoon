using UnityEngine;

public class MoneyObj : MonoBehaviour
{
    //무브 스피드
    [SerializeField] float moveSpeed = 3.0f;

    //도착 위치
    private Transform endPoint;

    //202600814
    //js.shin
    //SetEndPoint : EndPoint Set
    //para
    //endPoint : 도착 위치
    public void SetEndPoint(Transform endPoint)
    {
        this.endPoint = endPoint;       
    }

    void Update()
    {
        //도착 위치 x
        if (endPoint == null) return;
        
        //이동
        gameObject.transform.position = Vector3.MoveTowards(
                   gameObject.transform.position,
                   endPoint.position,
                   moveSpeed * Time.deltaTime
               );
        //도착
        if ((gameObject != null
            && Vector3.Distance(gameObject.transform.position, endPoint.position) < 0.01f))
        {
            Destroy(gameObject);
        }
    }
}
