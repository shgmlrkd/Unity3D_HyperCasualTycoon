using UnityEngine;

public class InGameEmployeeNPCInitializer : MonoBehaviour
{
    private void Start()
    {
        if (StateManager.Instance != null && !UIManager.Instance.IsNewGame)
        {
            StateManager.Instance.SpawnLoadedEmployees();
            Debug.Log("[InGameInitializer] 인게임 직원 NPC 스폰 완료!");
        }

        UIManager.Instance.SetNewGame(false);
    }
}
