using UnityEngine;
using System.Collections;
public class MoneyShooter : MonoBehaviour
{
    [SerializeField] private GameObject throwPrefab; // 던질 프리팹
    [SerializeField] private Transform startPoint;    // 던지는 시작 위치
    //[SerializeField] private Transform endPoint;
    

    [SerializeField] float speed = 0.5f;

    private Transform endPoint;

    //private Transform endPoint2;
    private bool isStart = false;
    public bool GetIsStart()
    {
        return isStart;
    }
    public void SetIsStart(bool isStart)
    {
        this.isStart = isStart; 
    }


    //private void Awake()
    //{
    //    endPoint2 = 
    //}

    public void SetEndPoint(Transform endPoin)
    {
        this.endPoint = endPoint;
    }

    void Update()
    {
        if (!isStart) return;
        GameObject instance = Instantiate(throwPrefab, startPoint.position, Quaternion.identity);
        StartCoroutine(MoveToDestination(instance));
           
    }

    private IEnumerator MoveToDestination(GameObject obj)
    {
        isStart = false;
        while (obj != null && Vector3.Distance(obj.transform.position, endPoint.position) > 0.01f)
        {
            // 도착지까지 매 프레임 이동
            obj.transform.position = Vector3.MoveTowards(
                obj.transform.position,
                endPoint.position,
                speed * Time.deltaTime
            );
            yield return 0.5f;
        }
        Destroy(obj);
        isStart = true;
    }
}
