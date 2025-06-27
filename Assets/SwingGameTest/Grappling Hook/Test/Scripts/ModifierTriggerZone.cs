using UnityEngine;

public class ModifierTriggerZone : MonoBehaviour
{
    [Header("Modifier Settings")]
    public bool useBouncy;
    public bool useLowGravity;
    public bool useFrictionless;

    private float originalGravityScale;
    private bool valuesStored = false;

    private PhysicsMaterial2D originalMaterial;
    private Rigidbody2D playerRb;

    // Store the generated material so it can be reused and not leak memory
    private PhysicsMaterial2D generatedMaterial;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerRb = other.GetComponent<Rigidbody2D>();
            other.GetComponent<PlayerTriggerTrack>().playerInside = true;

            // Store original values
            if (playerRb != null && !valuesStored)
            {
                originalGravityScale = playerRb.gravityScale;
                valuesStored = true;
            }
            if (playerRb != null)
            {
                originalMaterial = playerRb.sharedMaterial;
            }

            // Create and configure a new material
            generatedMaterial = new PhysicsMaterial2D("GeneratedModifierMaterial");
            // Set defaults
            generatedMaterial.bounciness = 0f;
            generatedMaterial.friction = 0.4f;

            if (useBouncy)
            {
                generatedMaterial.bounciness = 1f;
            }
            if (useFrictionless)
            {
                generatedMaterial.friction = 0f;
            }
            // If both bouncy and frictionless, both properties will be set

            // Assign the generated material
            if (playerRb != null)
            {
                playerRb.sharedMaterial = generatedMaterial;
            }

            // Apply gravity modifier
            if (useLowGravity && playerRb != null)
            {
                playerRb.gravityScale = originalGravityScale * 0.5f;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Restore original material
            if (playerRb != null)
            {
                playerRb.sharedMaterial = originalMaterial;
            }
            if (playerRb != null)
            {
                playerRb.gravityScale = originalGravityScale;
            }
            valuesStored = false;

            // Optionally destroy the generated material to avoid memory leaks
            if (generatedMaterial != null)
            {
                Destroy(generatedMaterial);
                generatedMaterial = null;
            }
        }
    }
}
