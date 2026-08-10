using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnPositionData", menuName = "CustomerNPCSpawnPosition/SpawnPositionData")]
public class SpawnPositionData : ScriptableObject
{
    [SerializeField]
    private List<Vector3> positions;

    public List<Vector3> Positions => positions;
}