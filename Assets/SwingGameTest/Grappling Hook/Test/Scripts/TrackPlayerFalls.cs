using UnityEngine;

public class TrackPlayerFalls : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float yOffset = -10f; // Offset for the line
    [SerializeField] private BoxCollider2D fallLineCollider; // Invisible line collider
    [SerializeField] private float resetDelay = 0.5f; // Delay before resetting the line after a fall
    [SerializeField] private float fallThreshold = -0.1f; // Threshold for Y velocity to consider the player falling

    private Rigidbody2D playerRigidbody; // Reference to the player's Rigidbody2D
    private int fallCount = 0; // Tracks the number of falls

    // Public read-only property to expose the fall count
    public int FallCount => fallCount;

    private void Start()
    {
        // Get the Rigidbody2D component attached to the player
        playerRigidbody = GetComponent<Rigidbody2D>();
        if (playerRigidbody == null)
        {
            Debug.LogError("Rigidbody2D is not attached to the player.");
            enabled = false;
            return;
        }

        if (fallLineCollider == null)
        {
            Debug.LogError("Fall line collider is not assigned in TrackPlayerFalls.");
            enabled = false;
            return;
        }

    }

    private void Update()
    {
        // Check if the player is falling based on Y velocity
        if (playerRigidbody.velocity.y < fallThreshold)
        {
            // Player is falling, stop updating the fall line's Y position
        }
        else
        {
            // Player is not falling, update the fall line's Y position
            UpdateFallLineYPosition();
        }

        // Always update the fall line's X position
        UpdateFallLineXPosition();
    }

    private void UpdateFallLineYPosition()
    {
        // Update only the Y position of the fall line
        Vector3 newPosition = fallLineCollider.transform.position;
        newPosition.y = transform.position.y + yOffset; // Use the player's position directly
        fallLineCollider.transform.position = newPosition;
    }

    private void UpdateFallLineXPosition()
    {
        // Update only the X position of the fall line
        Vector3 newPosition = fallLineCollider.transform.position;
        newPosition.x = transform.position.x; // Use the player's position directly
        fallLineCollider.transform.position = newPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player crosses the fall line
        if (other.CompareTag("FallLine")) // Ensure the player is the one triggering the line
        {
            Debug.Log("Player has fallen!");
            fallCount++; // Increment the fall count
            PlayerPrefs.SetInt("FallCount", fallCount); // Save the fall count to PlayerPrefs
            StartCoroutine(ResetFallLine());
        }
    }

    private System.Collections.IEnumerator ResetFallLine()
    {
        // Wait for the reset delay
        yield return new WaitForSeconds(resetDelay);

        // Reset the fall line to follow the player again
        UpdateFallLineYPosition();
    }
}