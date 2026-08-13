using UnityEngine;

public class RestaurantEntrance : MonoBehaviour
{
    [SerializeField]
    private CameraFollow cameraFollow;

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Vector3 direction = other.transform.forward;

        bool isInterior = direction.z > 0.0f;

        cameraFollow.SetCameraView(isInterior);
    }
}
