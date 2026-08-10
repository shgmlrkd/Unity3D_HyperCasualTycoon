using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerCarrier))]
[RequireComponent(typeof(PlayerInteraction))]
public class PlayerManager : MonoBehaviour
{
    public PlayerMovement MovementManager { get; private set; }
    public PlayerCarrier CarrierManager { get; private set; }
    public PlayerInteraction InteractionManager { get; private set; }

    private void Awake()
    {
        MovementManager = GetComponent<PlayerMovement>();
        CarrierManager = GetComponent<PlayerCarrier>();
        InteractionManager = GetComponent<PlayerInteraction>();
    }
}
