using System;
using UnityEngine;

public class RestaurantUnlockStateManager : MonoBehaviour
{
    private UnlockChecker[] checkers;

    public event Action OnRestaurantUnlocked;

    private void Awake()
    {
        checkers = GetComponentsInChildren<UnlockChecker>(true);
    }


}
