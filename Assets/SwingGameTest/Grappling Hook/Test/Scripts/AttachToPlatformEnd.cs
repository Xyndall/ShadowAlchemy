using UnityEngine;

public class AttachToPlatformEnd : MonoBehaviour
{
    public BasicMovingPlatform platform;
    public Vector3 localEndOffset = new Vector3(1, 0, 0); // Set this to the end of your platform in local space

    void LateUpdate()
    {
        if (platform != null)
        {
            transform.position = platform.GetPlatformEndWorldPosition(localEndOffset);
            // Optionally, reset rotation if you want to ensure it never rotates
            // transform.rotation = Quaternion.identity;
        }
    }
}