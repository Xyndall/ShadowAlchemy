using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerHit : MonoBehaviour
{
    public static PlayerHit Instance { get; private set; }

    public int maxHP = 3;
    public float knockbackForce = 8f;
    public float knockbackUpward = 0.7f;
    public LayerMask trapLayer;
    public NewGrappleTest playerGrapple;
    public Rigidbody2D rb;
    public Animator animator;
    public Tilemap trapTilemap; // Assign in Inspector
    public Vector3 teleportPosition;

    public int currentHP;
    public bool isStunned = false;

    private GroundCheck groundCheck;
    private bool hasTakenAirborneDamage = false; // Add this flag

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (playerGrapple == null)
            playerGrapple = GetComponent<NewGrappleTest>();
        if (animator == null)
            animator = GetComponent<Animator>();
        groundCheck = GetComponent<GroundCheck>();
        if (groundCheck == null)
            Debug.LogError("GroundCheck component not found on player!");
        currentHP = maxHP;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & trapLayer) != 0 && !isStunned && !hasTakenAirborneDamage)
        {
            hasTakenAirborneDamage = true; // Set flag
            Vector3 hitPosition = transform.position; // Player's position at collision
            if (trapTilemap != null)
            {
                NewGrappleTest.instance.DisableGrapple(); // Disable grappling when hit by a trap
                Vector3Int cellPos = trapTilemap.WorldToCell(hitPosition);
                Vector3 tileWorldPos = trapTilemap.GetCellCenterWorld(cellPos);
                Debug.Log("Player hit trap at tile cell: " + cellPos + " world position: " + tileWorldPos);

                // Use tileWorldPos for knockback direction if needed:
                Vector2 knockbackDir = (transform.position - tileWorldPos).normalized;
                knockbackDir = (knockbackDir + Vector2.up * knockbackUpward).normalized;
                TakeDamage(knockbackDir);
            }
            else
            {
                // Fallback: use the old method
                NewGrappleTest.instance.DisableGrapple();// Disable grappling when hit by a trap
                Vector2 knockbackDir = (transform.position - other.transform.position).normalized;
                knockbackDir = (knockbackDir + Vector2.up * knockbackUpward).normalized;
                TakeDamage(knockbackDir);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & trapLayer) != 0 && !isStunned && !hasTakenAirborneDamage)
        {
            hasTakenAirborneDamage = true; // Set flag
            Vector3 hitPosition = transform.position; // Player's position at collision
            if (trapTilemap != null)
            {
                NewGrappleTest.instance.DisableGrapple(); // Disable grappling when hit by a trap
                Vector3Int cellPos = trapTilemap.WorldToCell(hitPosition);
                Vector3 tileWorldPos = trapTilemap.GetCellCenterWorld(cellPos);
                Debug.Log("Player hit trap at tile cell: " + cellPos + " world position: " + tileWorldPos);

                // Use tileWorldPos for knockback direction if needed:
                Vector2 knockbackDir = (transform.position - tileWorldPos).normalized;
                knockbackDir = (knockbackDir + Vector2.up * knockbackUpward).normalized;
                TakeDamage(knockbackDir);
            }
            else
            {
                // Fallback: use the old method
                NewGrappleTest.instance.DisableGrapple();// Disable grappling when hit by a trap
                Vector2 knockbackDir = (transform.position - other.transform.position).normalized;
                knockbackDir = (knockbackDir + Vector2.up * knockbackUpward).normalized;
                TakeDamage(knockbackDir);
            }
        }
    }

    private void TakeDamage(Vector2 knockbackDir)
    {
        currentHP--;

        // Knockback
        rb.velocity = Vector2.zero;
        rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);

        Debug.Log("Player hit! HP: " + currentHP);

        // Disable movement/input
        isStunned = true;

        // Play hit animation
        if (animator != null)
            animator.SetBool("IsStunned", isStunned);

        if (currentHP == 0)
        {
            StartCoroutine(TeleportAndReset());
        }
        else
        {
            StartCoroutine(WaitUntilGrounded());
        }
    }

    private IEnumerator TeleportAndReset()
    {
        yield return new WaitForSeconds(0.2f);

        transform.position = teleportPosition;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        currentHP = maxHP;

        yield return StartCoroutine(WaitUntilGrounded());
    }

    private IEnumerator WaitUntilGrounded()
    {
        // Wait until the player is grounded using GroundCheck
        while (groundCheck == null || !groundCheck.IsGrounded)
            yield return null;

        isStunned = false;
        hasTakenAirborneDamage = false; // Reset flag when grounded

        // Stop hit animation
        if (animator != null)
            animator.SetBool("IsStunned", isStunned);
    }

    public void SetTeleportPosition(Vector3 newPosition)
    {
        teleportPosition = newPosition;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a sphere at the teleport position for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(teleportPosition, 0.2f);
    }
}
