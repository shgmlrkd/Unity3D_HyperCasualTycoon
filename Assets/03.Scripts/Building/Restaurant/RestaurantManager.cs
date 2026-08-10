using System.Collections.Generic;
using UnityEngine;

public class RestaurantManager : MonoBehaviour
{
    // 레스토랑의 해금을 하기 위한 모든 오브젝트들을 리스트에 순서대로 넣는다
    // 손님 NPC들에게 필요한 의자정보를 위 리스트에서 찾아서 리스트로 만들어 제공한다.

    [Header("발판 리스트")]
    [SerializeField]
    private List<GameObject> unlockCheckers;

    [Header("해금 오브젝트 리스트")]
    [SerializeField]
    private List<GameObject> restaurantObjs;

    private int index = 0;

    private void Awake()
    {
        Initialize();

        unlockCheckers[0].SetActive(true);
    }

    private void Initialize()
    {
        foreach (GameObject obj in unlockCheckers)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in restaurantObjs)
        {
            obj.SetActive(false);
        }
    }

    public void Unlocked()
    {
        if (index >= restaurantObjs.Count)
            return;

        restaurantObjs[index].SetActive(true);

        index++;

        if (index < unlockCheckers.Count)
        {
            unlockCheckers[index].SetActive(true);
        }
    }
}
