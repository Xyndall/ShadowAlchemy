using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformChildCheck : MonoBehaviour
{
    private GroundCheck groundCheck;
    private Transform currentPlatform;
    public LayerMask platformLayerMask; // Select platform layers in Inspector

    void Awake()
    {
        groundCheck = GetComponent<GroundCheck>();
        if (groundCheck == null)
        {
            Debug.LogError("GroundCheck component not found on player!");
        }
    }

    void Update()
    {
        if (groundCheck == null)
            return;

        // Check if grounded and the grounded layer is in the platformLayerMask
        if (groundCheck.IsGrounded && groundCheck.CurrentPlatform != null &&
            ((1 << groundCheck.GroundedLayer) & platformLayerMask.value) != 0)
        {
            if (currentPlatform != groundCheck.CurrentPlatform)
            {
                transform.SetParent(groundCheck.CurrentPlatform);
                currentPlatform = groundCheck.CurrentPlatform;
            }
        }
        else
        {
            if (currentPlatform != null)
            {
                transform.SetParent(null);
                currentPlatform = null;
            }
        }
    }
}
