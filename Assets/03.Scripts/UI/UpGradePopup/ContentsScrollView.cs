using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ContentsScrollView : MonoBehaviour
{
    [SerializeField] private Transform contentTransform; // Scroll View -> Viewport -> Content 오브젝트 할당
    [SerializeField] private GameObject itemPrefab;       // 스크롤 뷰에 넣을 아이템 프리팹 할당


    //content date list
    private ContentListData contents;


    //202600812
    //js.shin
    //SetContentDate : content date list set

    public void SetContentDate(ContentListData contents)
    {
        this. contents = contents;  
    }


    //202600812
    //js.shin
    //CreateScrollItems : Create Scroll Items
    public void CreateScrollItems()
    {
        // 기존에 생성되어 있던 아이템이 있다면 삭제 (초기화 필요할 때 사용)
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }
        
        // 반복문을 돌며 프리팹 동적 생성
        for (int i = 0; i < contents.ContentList.Count; i++)
        {
            // 1. 프리팹을 생성하면서 부모(Content)를 지정합니다.
            GameObject newItem = Instantiate(itemPrefab, contentTransform);

            // 2. 프리팹 내부의 컴포넌트를 가져와 데이터를 변경합니다.
            // (예: 아이템 스크립트가 있다면 데이터를 세팅)
            Content itemScript = newItem.GetComponent<Content>();
            if (itemScript != null)
            {
                //content data set, load
                itemScript.SetDate(contents.ContentList[i]);
            }
        }
    }
}
