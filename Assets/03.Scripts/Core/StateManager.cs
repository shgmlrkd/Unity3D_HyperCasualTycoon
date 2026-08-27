using System;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoSingleton<StateManager>
{

    
    //모든 level State 
    private Dictionary<RestaurantType, Dictionary<TypeId, StateData>> dicData =
        new Dictionary<RestaurantType, Dictionary<TypeId, StateData>>();

    private ContentsListSOData contentsList;// Resources Load data

    public event Action<TypeId, int> OnUpgradeChanged;

    protected override void Awake()
    {
        base.Awake();

        //Resources Load : basic state
        contentsList = Resources.Load<ContentsListSOData>("UI/Content/ContentsListSOData");

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

    //202600820
    //js.shin
    //MakeEmployee : Make Employee
    //Para :
    //  restaurantType : restaurant Type
    //  typeId : type Id
    private void MakeEmployee(RestaurantType restaurantType, TypeId typeId)
    {
        //Resources data 
        foreach (var item in contentsList.ContentsList)
        {
            //restaurant Type 비교
            if (item.type == restaurantType)
            {
                //contents
                foreach (var content in item.contents.Contents)
                {
                    //Type Id 비교
                    if (content.TypeId == typeId)
                    {

                        //Employee Prefab 생성
                        GameObject newItem = Instantiate(content.EmployeePrefab, content.Position, Quaternion.identity);

                        // 프리팹 내부의 컴포넌트를 가져와 데이터를 변경합니다.
                        EmployeeNPC Employee = newItem.GetComponent<EmployeeNPC>();
                        if (Employee != null)
                        {
                            //Employee.SetEmployee(stateData.FoodType);

                            //Employee FoodType setting
                            Employee.SetEmployee((int)content.FoodType, content.TypeId);
                        }
                    }

                }
            }            
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
    //   addLevel : level count
    public void AddUpgradeLevel(RestaurantType restaurantType, TypeId typeId, int addLevel)
    {
        StateData state = dicData[restaurantType][typeId];

        // 최대 레벨이면 종료
        if (state.UpgradCount >= state.UpgradeMaxCount)
            return;

        // 최초 직원 해금 여부 확인
        bool isFirstUnlock = typeId != TypeId.Player &&
                             state.UpgradCount == 0 &&
                             addLevel > 0;

        // 레벨 증가
        state.UpgradCount += addLevel;

        // 최초 직원 해금
        if (isFirstUnlock)
        {
            MakeEmployee(restaurantType, typeId);
        }

        // 업그레이드 이벤트 발생
        OnUpgradeChanged?.Invoke(typeId, state.UpgradCount);
    }

    //202600820
    //js.shin
    //GetUpgradeLevel : Get Upgrade Level(NPC)
    //Para :
    //  FoodType : Food Type
    //return : level , -1 : 없음
    public int GetUpgradeLevel(FoodType foodType)
    {
        //모든 데이터 루프
        foreach (KeyValuePair<RestaurantType, Dictionary<TypeId, StateData>> dicState in dicData)
        {
            //StateData 루프
            foreach (KeyValuePair<TypeId, StateData> state in dicState.Value)
            {
                //FoodType 찾기
                if (state.Value.FoodType == foodType)
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

    public void ApplyLoadedLevels(SaveData saveData)
    {
        if (saveData == null) return;

        SetUpgradeLevelData(RestaurantType.PizzaHamburger, TypeId.Player, saveData.playerUpgradeLevel);
        SetUpgradeLevelData(RestaurantType.PizzaHamburger, TypeId.Employee01, saveData.employee01UpgradeLevel);
        SetUpgradeLevelData(RestaurantType.PizzaHamburger, TypeId.Employee02, saveData.employee02UpgradeLevel);

        SetUpgradeLevelData(RestaurantType.CakeIcecream, TypeId.Employee03, saveData.employee03UpgradeLevel);
        SetUpgradeLevelData(RestaurantType.CakeIcecream, TypeId.Employee04, saveData.employee04UpgradeLevel);

        Debug.Log("[StateManager] 불러온 업그레이드 레벨 적용 완료!");
    }

    private void SetUpgradeLevelData(RestaurantType restaurantType, TypeId typeId, int level)
    {
        if (dicData.ContainsKey(restaurantType) && dicData[restaurantType].ContainsKey(typeId))
        {
            StateData state = dicData[restaurantType][typeId];

            state.UpgradCount = level;
        }
    }

    // 직원 NPC 생성 함수 따로 분리
    public void SpawnLoadedEmployees()
    {
        foreach (KeyValuePair<RestaurantType, Dictionary<TypeId, StateData>> restaurantPair in dicData)
        {
            RestaurantType resType = restaurantPair.Key;

            foreach (KeyValuePair<TypeId, StateData> typePair in restaurantPair.Value)
            {
                TypeId typeId = typePair.Key;
                StateData state = typePair.Value;

                if (typeId != TypeId.Player && state.UpgradCount > 0)
                {
                    MakeEmployee(resType, typeId);
                    OnUpgradeChanged?.Invoke(typeId, state.UpgradCount);
                }
            }
        }
    }
}



//State class
public class StateData
{
    
    public TypeId TypeId { get; set; } //TypeID
    public FoodType FoodType { get; set; }//Food Types
    public int UpgradeMaxCount { get; set; }//max level
    public int UpgradCount { get; set; } //level
}
 
