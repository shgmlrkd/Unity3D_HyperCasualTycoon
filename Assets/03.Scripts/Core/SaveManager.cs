using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int money = 121;
    public int CurrentUnlockIndex = 0;

    // 구 기획의 데이터. 일단 지우지는 않고 남겨둠.
        public int currentDay = 1;
        public int gold = 120;
        public int reputation;
        public int visitorCount;
        public int festivalStage;
    
}

[System.Serializable]
public class OptionData
{
    public float masterVol = 1f;
    public float bgmVol = 1f;
    public float sfxVol = 1f;
}

public class SaveManager : MonoSingleton<SaveManager>
{
    private string saveFilePath;
    private string optionFilePath;

    public SaveData CurrentData { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        saveFilePath = Path.Combine(Application.persistentDataPath, "SaveData.json");
        optionFilePath = Path.Combine(Application.persistentDataPath, "OptionData.json");

        CurrentData = new SaveData();

        //LoadGameData();
    }

    public void PrepareNewGame()
    {
        CurrentData = new SaveData();
        //CurrentData.money = 121;
        //CurrentData.gold = 120;
        //CurrentData.CurrentUnlockIndex = 0;
        Debug.Log("[SaveManager] New Game 데이터 : " + $"Gold {CurrentData.gold}, Money {CurrentData.money}, UnlockIndex {CurrentData.CurrentUnlockIndex}");
        Debug.Log("[SaveManager] New Game 준비 완료 (기존 세이브 파일은 보존됨)");
    }

    public void SaveGameData()
    {
        if (CurrentData == null)
        {
            CurrentData = new SaveData();
        }

        if (CurrencyManager.Instance != null)
        {
            CurrentData.money = CurrencyManager.Instance.CurrentMoney;
            CurrentData.gold = CurrencyManager.Instance.CurrentGold;
        }

        //if (ReputationManager.Instance != null)
        //{
        //    CurrentData.reputation = ReputationManager.Instance.CurrentReputation;
        //}

        // 이거 그대로 쓰시면 해금 시스템 저장될거에요. - 노희강 -
        /*if (UnlockPointManager.Instance != null)
        {
            CurrentData.CurrentUnlockIndex = UnlockPointManager.Instance.CurrentUnlockPointIndex;
        }*/

        Debug.Log($"UnlockPointManager.Instance 잘 불러오나? : " + UnlockPointManager.Instance);

        if (UnlockPointManager.Instance != null)
        {
            Debug.Log($"UnlockPointManager.Instance 잘 불러와서 저장 중.");
            CurrentData.CurrentUnlockIndex = UnlockPointManager.Instance.CurrentUnlockPointIndex;
        }

        Debug.Log($"[SaveManager] 저장 직전 값들은 무엇일까요! 자, 봅시다 : " + $"Gold : {CurrentData.gold}, Money : {CurrentData.money}, UnlockIndex : {CurrentData.CurrentUnlockIndex}");

        string json = JsonUtility.ToJson(CurrentData, true);
        File.WriteAllText(saveFilePath, json);

        Debug.Log($"[SaveManager] 게임 진행 JSON 저장 완료! 경로: {saveFilePath}");
    }

    public bool LoadGameData()
    {
        Debug.Log($"로드 게임 시작");
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("[SaveManager] 저장된 게임 파일이 없어 초기 데이터를 사용합니다.");
            CurrentData = new SaveData();
            return false;
        }

        try
        {
            string json = File.ReadAllText(saveFilePath);
            //CurrentData = JsonUtility.FromJson<SaveData>(json);
            SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

            if(loadedData == null)
            {
                Debug.LogError("[SaveManager] 세이브 JSON 파싱 실패");
                CurrentData = new SaveData();
                return false;
            }

            CurrentData = loadedData;
            Debug.Log("CurrentData를 보자");
            Debug.Log(CurrentData);
            Debug.Log($"Gold {CurrentData.gold}, Money {CurrentData.money}, UnlockIndex {CurrentData.CurrentUnlockIndex}");

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[SaveManager] 세이브 로드 실패 : {e}");
            CurrentData = new SaveData();
            return false;
        }

        

        //if (CurrencyManager.Instance != null)
        //{
        //    int currentMoney = CurrencyManager.Instance.CurrentMoney;
        //    if (CurrentData.money > currentMoney) CurrencyManager.Instance.AddMoney(CurrentData.money - currentMoney);
        //    else if (CurrentData.money < currentMoney) CurrencyManager.Instance.TrySpendMoney(currentMoney - CurrentData.money);

        //            int currentGold = CurrencyManager.Instance.CurrentGold;
        //            if (CurrentData.gold > currentGold) CurrencyManager.Instance.AddGold(CurrentData.gold - currentGold);
        //            else if (CurrentData.gold < currentGold) CurrencyManager.Instance.TrySpendGold(currentGold - CurrentData.gold);
        //}

        //if (ReputationManager.Instance != null)
        //{
        //    int currentRep = ReputationManager.Instance.CurrentReputation;
        //    if (CurrentData.reputation > currentRep) ReputationManager.Instance.AddReputation(CurrentData.reputation - currentRep);
        //    else if (CurrentData.reputation < currentRep) ReputationManager.Instance.DecreaseReputation(currentRep - CurrentData.reputation);
        //}

        //Debug.Log($"UnlockPointManager.Instance");
        //Debug.Log(UnlockPointManager.Instance);
        //if (UnlockPointManager.Instance != null)
        //{
        //    Debug.Log($"UnlockPointManager.Instance 된 건가");
        //    UnlockPointManager.Instance.LoadUnlockPoint(CurrentData.CurrentUnlockIndex);
        //}

        //Debug.Log($"[SaveManager] 게임 진행 JSON 불러오기 0");
        //Debug.Log($"UnlockPointManager.Instance");
        //Debug.Log(UnlockPointManager.Instance);
        //if (UnlockPointManager.Instance != null)
        //{
        //    Debug.Log($"[SaveManager] 게임 진행 JSON 불러오기 1");
        //    UnlockPointManager.Instance.LoadUnlockPoint(CurrentData.CurrentUnlockIndex);
        //    Debug.Log($"[SaveManager] 게임 진행 JSON 불러오기 2");
        //}

        //Debug.Log($"[SaveManager] 게임 진행 JSON 불러오기 완료! (Day {CurrentData.currentDay})");
    }

    //public void AdvanceToNextDay()
    //{
    //    if (CurrentData == null) CurrentData = new SaveData();
    //    CurrentData.currentDay++;
    //    SaveGameData();
    //}

    public void SaveOptionData(float master, float bgm, float sfx)
    {
        OptionData data = new OptionData
        {
            masterVol = master,
            bgmVol = bgm,
            sfxVol = sfx
        };

        string newJson = JsonUtility.ToJson(data, true);
        File.WriteAllText(optionFilePath, newJson);

        Debug.Log($"[SaveManager] 옵션 JSON 저장 완료! 경로: {optionFilePath}");
    }

    public OptionData LoadOptionData()
    {
        if (!File.Exists(optionFilePath))
        {
            Debug.Log("[SaveManager] 옵션 파일이 없어 기본값(1.0)으로 생성합니다.");
            return new OptionData();
        }

        string json = File.ReadAllText(optionFilePath);
        Debug.Log("[SaveManager] 옵션 JSON 불러오기 완료!");
        return JsonUtility.FromJson<OptionData>(json);
    }

    //public void ResetGameData()
    //{
    //    if (CurrencyManager.Instance != null)
    //    {
    //        CurrencyManager.Instance.ResetData();
    //    }

    //    if (File.Exists(saveFilePath))
    //    {
    //        File.Delete(saveFilePath);
    //    }

    //    CurrentData = new SaveData();
    //    Debug.Log("[SaveManager] 인게임 데이터 및 세이브 파일 초기화 완료 (New Game)");
    //}

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        // SaveGameData();
    }
}