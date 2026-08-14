using UnityEngine;
using UnityEngine.Rendering;

public class Money : MonoBehaviour
{
    [SerializeField] private int money = 0;

    public void SetMoney(int money)
    {
        this.money = money;
    }

    public void DestroyMoney()
    {
        Destroy(gameObject);    
    }
}
