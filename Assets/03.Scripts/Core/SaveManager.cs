using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
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

    protected override void Awake()
    {
        base.Awake();
        saveFilePath = Path.Combine(Application.persistentDataPath, "SaveData.json");
        optionFilePath = Path.Combine(Application.persistentDataPath, "OptionData.json");
    }

    public void SaveGameData()
    {
        SaveData data = new SaveData();

        if (CurrencyManager.Instance != null) data.gold = CurrencyManager.Instance.CurrentGold;
        if (ReputationManager.Instance != null) data.reputation = ReputationManager.Instance.CurrentReputation;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);

        Debug.Log($"[SaveManager] 게임 진행 JSON 저장 완료! 경로: {saveFilePath}");
    }

    public void LoadGameData()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("[SaveManager] 저장된 게임 파일이 없습니다!");
            return;
        }

        string json = File.ReadAllText(saveFilePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        if (CurrencyManager.Instance != null)
        {
            int currentGold = CurrencyManager.Instance.CurrentGold;
            if (data.gold > currentGold) CurrencyManager.Instance.AddGold(data.gold - currentGold);
            else if (data.gold < currentGold) CurrencyManager.Instance.TrySpendGold(currentGold - data.gold);
        }

        if (ReputationManager.Instance != null)
        {
            int currentRep = ReputationManager.Instance.CurrentReputation;
            if (data.reputation > currentRep) ReputationManager.Instance.AddReputation(data.reputation - currentRep);
            else if (data.reputation < currentRep) ReputationManager.Instance.DecreaseReputation(currentRep - data.reputation);
        }

        Debug.Log("[SaveManager] 게임 진행 JSON 불러오기 완료!");
    }

    public void SaveOptionData(float master, float bgm, float sfx)
    {
        OptionData data = new OptionData
        {
            masterVol = master,
            bgmVol = bgm,
            sfxVol = sfx
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(optionFilePath, json);

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

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        // SaveGameData();
    }
}