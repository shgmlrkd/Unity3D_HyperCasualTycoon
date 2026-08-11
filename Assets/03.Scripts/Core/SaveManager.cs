using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int currentDay = 1;
    public int money;
    public int gold;
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

        LoadGameData();
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

        if (ReputationManager.Instance != null)
        {
            CurrentData.reputation = ReputationManager.Instance.CurrentReputation;
        }

        string json = JsonUtility.ToJson(CurrentData, true);
        File.WriteAllText(saveFilePath, json);

        Debug.Log($"[SaveManager] 게임 진행 JSON 저장 완료! 경로: {saveFilePath}");
    }

    public void LoadGameData()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("[SaveManager] 저장된 게임 파일이 없어 초기 데이터를 사용합니다.");
            CurrentData = new SaveData();
            return;
        }

        string json = File.ReadAllText(saveFilePath);
        CurrentData = JsonUtility.FromJson<SaveData>(json);

        if (CurrencyManager.Instance != null)
        {
            int currentMoney = CurrencyManager.Instance.CurrentMoney;
            if (CurrentData.money > currentMoney) CurrencyManager.Instance.AddMoney(CurrentData.money - currentMoney);
            else if (CurrentData.money < currentMoney) CurrencyManager.Instance.TrySpendMoney(currentMoney - CurrentData.money);

            int currentGold = CurrencyManager.Instance.CurrentGold;
            if (CurrentData.gold > currentGold) CurrencyManager.Instance.AddGold(CurrentData.gold - currentGold);
            else if (CurrentData.gold < currentGold) CurrencyManager.Instance.TrySpendGold(currentGold - CurrentData.gold);
        }

        if (ReputationManager.Instance != null)
        {
            int currentRep = ReputationManager.Instance.CurrentReputation;
            if (CurrentData.reputation > currentRep) ReputationManager.Instance.AddReputation(CurrentData.reputation - currentRep);
            else if (CurrentData.reputation < currentRep) ReputationManager.Instance.DecreaseReputation(currentRep - CurrentData.reputation);
        }

        Debug.Log($"[SaveManager] 게임 진행 JSON 불러오기 완료! (Day {CurrentData.currentDay})");
    }

    public void AdvanceToNextDay()
    {
        if (CurrentData == null) CurrentData = new SaveData();
        CurrentData.currentDay++;
        SaveGameData();
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

    public void ResetGameData()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.ResetData();
        }

        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }

        CurrentData = new SaveData();
        Debug.Log("[SaveManager] 인게임 데이터 및 세이브 파일 초기화 완료 (New Game)");
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        // SaveGameData();
    }
}