using UnityEngine;
using UnityEngine.UI;

public class CircleGauge : MonoBehaviour
{
    [SerializeField] private GameObject circleGauge;
    [SerializeField] private Image itemSlider; //게이지 이미지
    [SerializeField] private Image foodImg; // 푸드이지미 -  기본은 이미지 없음
    [SerializeField] private float itemCoolDownTime = 5.0f; //쿨타임
    float updateTime = 0.0f; //타임

    

    private void Update()
    {
        //쿨타임 초과시 초기화
        if (updateTime > itemCoolDownTime)
        {
            updateTime = 0.0f;
            itemSlider.fillAmount = 0.0f;
        }
        else
        {
            //타임
            updateTime = updateTime + Time.deltaTime;
            //itemSlider.fillAmount = 1.0f - (Mathf.Lerp(0, 100, updateTime / itemCoolDownTime) / 100);

            //fillAmount 증가
            itemSlider.fillAmount = (Mathf.Lerp(0, 100, updateTime / itemCoolDownTime) / 100);
        }
    }
    //public void SetActiveGauge()
    //{
    //    Debug.Log("TEst");
    //    //circleGauge.SetActive(true); 
    //}
    //20260809
    //JS.S
    //SetFoodImg : foot img setting
    //para 
    //image : food Img
    public void SetFoodImg(Sprite image)
    {
        if(image==null)return;
        foodImg.sprite = image;
    }
    public void Test()
    {
        Debug.Log("TEst");
       // circleGauge.SetActive(true);
    }

}
