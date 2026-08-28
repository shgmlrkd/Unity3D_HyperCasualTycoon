using System;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int money = 121;
    public int gold = 120;
    public int CurrentUnlockIndex = 0;

    public int playerUpgradeLevel = 1;      // 기본값 1로 하면 되나? 몇으로 해야되지?
    public int employee01UpgradeLevel = 0;
    public int employee02UpgradeLevel = 0;
    public int employee03UpgradeLevel = 0;
    public int employee04UpgradeLevel = 0;

    // 구 기획의 데이터. 일단 지우지는 않고 남겨둠.
    public int currentDay = 1;
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

    public bool IsDirty { get; private set; } = false;

    protected override void Awake()
    {
        base.Awake();
        saveFilePath = Path.Combine(Application.persistentDataPath, "SaveData.json");
        optionFilePath = Path.Combine(Application.persistentDataPath, "OptionData.json");

        CurrentData = new SaveData();
    }

    public void SetDirty()
    {
        IsDirty = true;
        Debug.Log("[SaveManager] 데이터 변경 감지됨 (IsDirty = true)");
    }

    public bool HasSaveFile()
    {
        return File.Exists(saveFilePath);
    }

    public void PrepareNewGame()
    {
        CurrentData = new SaveData();
        IsDirty = false;
        
        if (StateManager.Instance != null)
        {
            StateManager.Instance.ResetToDefaultLevels();
        }
        
        Debug.Log("[SaveManager] New Game 준비 완료 (IsDirty = false)");
    }

    public bool SaveGameData()
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

        if (UnlockPointManager.Instance != null)
        {
            CurrentData.CurrentUnlockIndex = UnlockPointManager.Instance.CurrentUnlockPointIndex;
        }

        if (StateManager.Instance != null)
        {
            CurrentData.playerUpgradeLevel = StateManager.Instance.GetPopupUpgradeLevel(RestaurantType.PizzaHamburger, TypeId.Player);
            CurrentData.employee01UpgradeLevel = StateManager.Instance.GetPopupUpgradeLevel(RestaurantType.PizzaHamburger, TypeId.Employee01);
            CurrentData.employee02UpgradeLevel = StateManager.Instance.GetPopupUpgradeLevel(RestaurantType.PizzaHamburger, TypeId.Employee02);
            CurrentData.employee03UpgradeLevel = StateManager.Instance.GetPopupUpgradeLevel(RestaurantType.CakeIcecream, TypeId.Employee03);
            CurrentData.employee04UpgradeLevel = StateManager.Instance.GetPopupUpgradeLevel(RestaurantType.CakeIcecream, TypeId.Employee04);
        }

        try
        {
            string json = JsonUtility.ToJson(CurrentData, true);
            File.WriteAllText(saveFilePath, json);

            IsDirty = false;
            Debug.Log($"[SaveManager] 게임 진행 JSON 저장 성공! 경로: {saveFilePath}");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveManager] 게임 저장 실패: {e.Message}");
            return false;
        }
    }

    public bool LoadGameData()
    {
        if (!HasSaveFile())
        {
            Debug.LogWarning("[SaveManager] 저장된 게임 파일이 없어 초기 데이터를 사용합니다.");
            CurrentData = new SaveData();
            IsDirty = false;
            return false;
        }

        try
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

            if (loadedData == null)
            {
                Debug.LogError("[SaveManager] 세이브 JSON 파싱 실패");
                CurrentData = new SaveData();
                IsDirty = false;
                return false;
            }

            CurrentData = loadedData;
            IsDirty = false;

            if (StateManager.Instance != null)
            {
                StateManager.Instance.ApplyLoadedLevels(CurrentData);
            }

            Debug.Log($"[SaveManager] 세이브 로드 성공! Money: {CurrentData.money}, Gold: {CurrentData.gold}, UnlockIndex: {CurrentData.CurrentUnlockIndex}");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveManager] 세이브 로드 실패 : {e.Message}");
            CurrentData = new SaveData();
            IsDirty = false;
            return false;
        }
    }

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
    }

    public OptionData LoadOptionData()
    {
        if (!File.Exists(optionFilePath))
        {
            return new OptionData();
        }

        string json = File.ReadAllText(optionFilePath);
        return JsonUtility.FromJson<OptionData>(json);
    }
}