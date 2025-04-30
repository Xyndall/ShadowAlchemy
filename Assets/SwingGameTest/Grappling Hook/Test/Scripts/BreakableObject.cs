using System.Collections;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject piecePrefab; // Prefab for the broken pieces
    [SerializeField] private int pieceCount = 5; // Number of pieces to spawn
    [SerializeField] private float explosionForce = 5f; // Force applied to the pieces
    [SerializeField] private float destroyDelay = 2f; // Time before the pieces are destroyed

    [SerializeField] private string uniqueID; // Unique identifier for this object

    // Public property to expose the uniqueID
    public string UniqueID => uniqueID;

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer

    private void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer is not attached to the BreakableObject.");
        }

        // Check if this object is marked as broken in PlayerPrefs
        if (PlayerPrefs.GetInt($"BreakableObject_{uniqueID}", 0) == 1)
        {
            // If the object is broken, deactivate it
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Disable the SpriteRenderer on the breakable object
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
            }

            // Get the player's Rigidbody2D to determine its velocity
            Rigidbody2D playerRigidbody = other.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                Vector2 playerVelocity = playerRigidbody.velocity;

                // Break the object into pieces
                BreakIntoPieces(playerVelocity);
            }

            // Mark the object as broken in PlayerPrefs
            PlayerPrefs.SetInt($"BreakableObject_{uniqueID}", 1);
            PlayerPrefs.Save();

            // Destroy the original object after the pieces are created
            Destroy(gameObject, destroyDelay);
        }
    }

    private void BreakIntoPieces(Vector2 playerVelocity)
    {
        for (int i = 0; i < pieceCount; i++)
        {
            // Instantiate a piece at the object's position
            GameObject piece = Instantiate(piecePrefab, transform.position, Quaternion.identity);

            // Get the Rigidbody2D of the piece to apply force
            Rigidbody2D pieceRigidbody = piece.GetComponent<Rigidbody2D>();
            if (pieceRigidbody != null)
            {
                // Apply force to the piece in the direction of the player's velocity with some randomness
                Vector2 randomDirection = playerVelocity.normalized + new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
                pieceRigidbody.AddForce(randomDirection * explosionForce, ForceMode2D.Impulse);
            }

            // Destroy the piece after the specified delay
            Destroy(piece, destroyDelay);
        }
    }
}