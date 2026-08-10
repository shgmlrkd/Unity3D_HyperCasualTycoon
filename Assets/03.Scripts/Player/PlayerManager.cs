using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Carrier))]
[RequireComponent(typeof(PlayerInteraction))]
public class PlayerManager : MonoBehaviour
{
    public PlayerMovement MovementManager { get; private set; }
    public Carrier CarrierManager { get; private set; }
    public PlayerInteraction InteractionManager { get; private set; }

    private void Awake()
    {
        MovementManager = GetComponent<PlayerMovement>();
        CarrierManager = GetComponent<Carrier>();
        InteractionManager = GetComponent<PlayerInteraction>();
    }
}
