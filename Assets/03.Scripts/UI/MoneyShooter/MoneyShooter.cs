using UnityEngine;
public class MoneyShooter : MonoBehaviour
{
    [SerializeField] private GameObject moneyPrefab; // 머니 프리팹

    //202600814
    //js.shin
    //ShootMoneny : moneyPrefab 생성
    //para
    //endPoint : 도착 위치
    public void ShootMoneny(Transform endPoint)
    {
        //moneyPrefab 생성
        GameObject instance = Instantiate(moneyPrefab, gameObject.transform.position, Quaternion.identity);
        //moneyPrefab 
        MoneyObj moneyObj = instance.GetComponent<MoneyObj>();
        //moneyPrefab EndPoint setting
        moneyObj.SetEndPoint(endPoint);  
    }
}
