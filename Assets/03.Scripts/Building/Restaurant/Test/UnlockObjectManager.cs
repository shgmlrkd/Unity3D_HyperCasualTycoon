using System;
using UnityEngine;

public class UnlockObjectManager : MonoBehaviour
{
    [SerializeField]
    private UnlockPointManager unlockPointmanager;

    private UnlockPoint[] unlockPoints;
    private UnlockableObject[] unlockableObjects;

    private void Awake()
    {
        unlockableObjects = GetComponentsInChildren<UnlockableObject>(true);
    }

    private void Start()
    {
        unlockPoints = unlockPointmanager.UnlockPoints;
        LoadUnlockObject(unlockPointmanager.CurrentUnlockPointIndex);
    }

    private void LoadUnlockObject(int currentUnlockIndex)
    {
        int index = 0;

        for (int i = 0; i < currentUnlockIndex; i++)
        {
            while(true)
            {
                if (unlockPoints[i] == unlockableObjects[index].UnlockPoint)
                {
                    unlockableObjects[index].UnlockObject();
                    index++;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
