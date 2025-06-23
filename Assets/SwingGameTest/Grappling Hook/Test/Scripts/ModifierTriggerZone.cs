using UnityEngine;

public class ModifierTriggerZone : MonoBehaviour
{
    [Header("Modifier Settings")]
    public bool useBouncy;
    public bool useLowGravity;
    public bool useFrictionless;

    private float originalBounciness;
    private float originalFriction;
    private float originalGravityScale;
    private bool valuesStored = false;

    private PhysicsMaterial2D playerMaterial;
    private Rigidbody2D playerRb;
    private Collider2D playerCollider;
    private bool colliderTemporarilyDisabled = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerTriggerTrack>().playerInside == true && !colliderTemporarilyDisabled)
        {
            
            playerRb = other.GetComponent<Rigidbody2D>();
            playerCollider = other.GetComponent<Collider2D>();
            other.GetComponent<PlayerTriggerTrack>().playerInside = true;
            // Store original values
            if (playerRb != null && !valuesStored)
            {
                originalGravityScale = playerRb.gravityScale;
                valuesStored = true;
            }
            if (playerCollider != null)
            {
                playerMaterial = playerRb.sharedMaterial;
                if (playerMaterial != null)
                {
                    originalBounciness = playerMaterial.bounciness;
                    originalFriction = playerMaterial.friction;
                }
            }

            // Apply modifiers
            if (useBouncy && playerMaterial != null)
            {
                playerMaterial.bounciness = 1f;
                playerMaterial.friction = 0f;
            }
            if (useFrictionless && playerMaterial != null)
            {
                playerMaterial.friction = 0f;
            }
            if (useLowGravity && playerRb != null)
            {
                playerRb.gravityScale = originalGravityScale * 0.5f;
            }

            // Disable the collider
            if (playerCollider != null)
            {
                playerCollider.enabled = false;
                colliderTemporarilyDisabled = true;
                // Start coroutine to re-enable after a short delay
                StartCoroutine(ReenableColliderAfterDelay(playerCollider, 0.1f));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerTriggerTrack>().playerInside == false && !colliderTemporarilyDisabled)
        {
            
            // Restore original values
            if (playerMaterial != null)
            {
                playerMaterial.bounciness = originalBounciness;
                playerMaterial.friction = originalFriction;
            }
            if (playerRb != null)
            {
                playerRb.gravityScale = originalGravityScale;
            }
            valuesStored = false;

            // Disable the collider
            if (playerCollider != null)
            {
                playerCollider.enabled = false;
                colliderTemporarilyDisabled = true;
                StartCoroutine(ReenableColliderAfterDelay(playerCollider, 0.1f));
            }
        }
    }

    private System.Collections.IEnumerator ReenableColliderAfterDelay(Collider2D collider, float delay)
    {
        yield return new WaitForSeconds(delay);
        collider.enabled = true;
        colliderTemporarilyDisabled = false;
    }
}
