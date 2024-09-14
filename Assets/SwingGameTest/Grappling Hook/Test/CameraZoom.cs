using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public GrappleRope grappleScript;
    public float zoomInSize = 5f;  // The camera size when zoomed in
    public float zoomOutSize = 8f; // The camera size when zoomed out
    public float zoomSpeed = 2f;   // The speed at which the camera zooms
    Rigidbody2D playerRigidbody;  // Reference to the player's Rigidbody2D

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check if the player is moving
        if (playerRigidbody.velocity.magnitude > 0.1f && grappleScript.isGrappling)
        {
            // Zoom out
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(
                virtualCamera.m_Lens.OrthographicSize,
                zoomOutSize,
                Time.deltaTime * zoomSpeed
            );
        }
        else if(!grappleScript.isGrappling)
        {
            // Zoom in
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(
                virtualCamera.m_Lens.OrthographicSize,
                zoomInSize,
                Time.deltaTime * zoomSpeed
            );
        }
    }
}

