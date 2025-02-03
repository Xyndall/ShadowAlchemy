using UnityEngine;

public class BoundaryTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider is the player (or assign a specific tag to the player)
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Get the Rigidbody2D of the player
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            Debug.Log(rb);
            if (rb != null)
            {
                // Reverse the velocity (same speed, opposite direction)
                rb.velocity = -rb.velocity;
            }
        }
    }
}
