using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Header("Ground Check Settings")]
    public Transform groundCheckPoint; // Assign a child transform at the player's feet
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer; // Assign the platform/ground layer

    public bool IsGrounded { get; private set; }
    public Transform CurrentPlatform { get; private set; }
    public int GroundedLayer { get; private set; } // <-- Add this

    void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
        IsGrounded = hit != null;
        CurrentPlatform = IsGrounded ? hit.transform : null;
        GroundedLayer = IsGrounded ? hit.gameObject.layer : -1; // <-- Store the layer
    }

    // Optional: Draw ground check gizmo in editor
    void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}