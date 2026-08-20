

using System.Collections.Generic;

using UnityEngine;

public class StateManager : MonoSingleton<StateManager>
{
    //모든 level State 
    private Dictionary<RestaurantType, Dictionary<TypeId, StateData>> dicData =
        new Dictionary<RestaurantType, Dictionary<TypeId, StateData>>();

    protected override void Awake()
    {
        base.Awake();

        //Resources Load : basic state
        ContentsListSOData contentsList = Resources.Load<ContentsListSOData>("UI/Content/ContentsListSOData");

        foreach (var item in contentsList.ContentsList)
        {
            //타입 ID 별 Dictionary
            Dictionary<TypeId, StateData> dicState = new Dictionary<TypeId, StateData>();
            foreach (var content in item.contents.Contents)
            {
                //StateData class
                StateData state = new StateData();
                //type Id
                state.TypeId = content.TypeId;
                //foot type
                state.FoodType = content.FoodType;
                //Upgrade Max Count
                state.UpgradeMaxCount = content.UpgradeMaxCount;
                //레벨 player 외에 초기 레벨 0
                state.UpgradCount = content.TypeId == TypeId.Player ? 1 : 0;
                //타입 ID 별 state 저장
                dicState.Add(content.TypeId, state);
            }
            //Restaurant Type별 데이터 저장
            dicData.Add(item.type, dicState);
        }
    }

    //save 데이터 load 
    //public void LoadData(Dictionary<PopupType, Dictionary<string, StateData>> dicData)
    //{
    //    this.dicData.Clear();   
    //}

    //202600820
    //js.shin
    //GetPopupUpgradeLevel : Get Popup Upgrade Level
    //Para :
    //  restaurantType : restaurant Type
    //  TypeId : Type Id
    //return : level
    public int GetPopupUpgradeLevel(RestaurantType restaurantType, TypeId TypeId)
    {
        //restaurantType - TypeId 레벨
        return dicData[restaurantType][TypeId].UpgradCount;
    }

    //202600820
    //js.shin
    //AddUpgradeLevel : Add Upgrade Level
    //Para :
    //  restaurantType : restaurant Type
    //  TypeId : Type Id
    public void AddUpgradeLevel(RestaurantType restaurantType, TypeId TypeId)
    {
        //max 레벨 보다 작으면
        if (dicData[restaurantType][TypeId].UpgradeMaxCount <= dicData[restaurantType][TypeId].UpgradCount) return;
        //restaurantType - TypeId 레벨 업그레이드
        dicData[restaurantType][TypeId].UpgradCount += 1;
    }

    //202600820
    //js.shin
    //GetUpgradeLevel : Get Upgrade Level(NPC)
    //Para :
    //  FoodType : Food Type
    //return : level , -1 : 없음
    public int GetUpgradeLevel(FoodType FoodType)
    {
        //모든 데이터 루프
        foreach (KeyValuePair<RestaurantType, Dictionary<TypeId, StateData>> dicState in dicData)
        {
            //StateData 루프
            foreach (KeyValuePair<TypeId, StateData> state in dicState.Value)
            {
                //FoodType 찾기
                if (state.Value.FoodType == FoodType)
                {
                    //level
                    return state.Value.UpgradCount;
                }
            }
        }
        return -1;
    }
    //202600820
    //js.shin
    //GetPlayerUpgradeLevel : Get Upgrade Level(Player)
    //return : level , -1 : 없음
    public int GetPlayerUpgradeLevel()
    {
        //모든 데이터 루프
        foreach (KeyValuePair<RestaurantType, Dictionary<TypeId, StateData>> dicState in dicData)
        {
            //StateData 루프
            foreach (KeyValuePair<TypeId, StateData> state in dicState.Value)
            {
                //player 찾기
                if (state.Value.TypeId == TypeId.Player)
                {
                    //level
                    return state.Value.UpgradCount;
                }
            }
        }
        return -1;
    }



}
//State class
public class StateData
{
    public TypeId TypeId { get; set; } //TypeID
    public FoodType FoodType { get; set; }//Food Types
    public int UpgradeMaxCount { get; set; }//max level
    public int UpgradCount { get; set; }// level
}
