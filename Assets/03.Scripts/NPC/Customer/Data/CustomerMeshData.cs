using UnityEngine;

[CreateAssetMenu(fileName = "CustomerDataSO", menuName = "CustomerNPC/CustomerDataSO")]
public class CustomerMeshData : ScriptableObject
{
    [SerializeField]
    private Mesh[] meshes;

    public Mesh GetRandomMesh()
    {
        return meshes[Random.Range(0, meshes.Length)];
    }
}