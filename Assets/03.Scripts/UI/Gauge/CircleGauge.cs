using UnityEngine;
using UnityEngine.UI;


public class CircleGauge : MonoBehaviour
{
    [SerializeField] private Image itemSlider; //게이지 이미지
    [SerializeField] private Image foodImg; // 푸드이지미 -  기본은 이미지 없음
    [SerializeField] private float itemCoolDownTime = 5.0f; //쿨타임
    private float updateTime = 0.0f; //타임
    //private bool complit = false; //게이지 완료
    public bool Complit {  get; set; }



    private void Awake()
    {
        //비활성화
        gameObject.SetActive(false);
        Complit = false;
    }

    private void Update()
    {
        if (Complit) return;
        //쿨타임 초과시 초기화
        if (updateTime > itemCoolDownTime)
        {
            //시간, fillAmount 초기화
            SetResetData();
        }
        else
        {
            //타임
            updateTime = updateTime + Time.deltaTime;
            //itemSlider.fillAmount = 1.0f - (Mathf.Lerp(0, 100, updateTime / itemCoolDownTime) / 100);

            //fillAmount 증가
            itemSlider.fillAmount = (Mathf.Lerp(0, 100, updateTime / itemCoolDownTime) / 100);
            //게이지 완료
            if (1 <= itemSlider.fillAmount)
            {
                Complit = true;
            }
        }
    }
    //20260811
    //JS.S
    //SetActiveGauge : 활성화
    //para 
    //active : true, false
    public void SetActiveGauge(bool active)
    {
        gameObject.SetActive(active);
    }
    //20260809
    //JS.S
    //SetFoodImg : foot img setting
    //para 
    //image : food Img
    public void SetFoodImg(Sprite image)
    {
        foodImg.sprite = image;
    }
    //20260811
    //JS.S
    //SetResetData : 시간, fillAmount 초기화
    public void SetResetData()
    {
        updateTime = 0.0f;
        itemSlider.fillAmount = 0.0f;
    }

    ////20260811
    ////JS.S
    ////GetComplit : Get Complit
    //public bool GetComplit()
    //{
    //    return complit;
    //}
    ////20260811
    ////JS.S
    ////SetComplit : Set Complit
    //public void SetComplit(bool complit)
    //{
    //    this.complit = complit;
    //}



}
