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
    private float stunTimer = 0f;
    public float maxStunDuration = 10f; // seconds

    public AudioSource audioSource; // Assign in Inspector or via code
    public AudioClip hitSound;      // Assign your hit sound in Inspector

    private float lastDamageTime = 0f;
    public float damageCooldown = 0.5f; // seconds

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

        SetTeleportPosition(new Vector3(PlayerPrefs.GetFloat(SaveManager.TeleportX), PlayerPrefs.GetFloat(SaveManager.TeleportY), PlayerPrefs.GetFloat(SaveManager.TeleportZ))); // Default teleport position

    }

    private void Update()
    {
        if (isStunned)
        {
            stunTimer += Time.deltaTime;
            if (stunTimer >= maxStunDuration)
            {
                isStunned = false;
                stunTimer = 0f;
                if (animator != null)
                    animator.SetBool("IsStunned", false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & trapLayer) != 0 && !isStunned && !hasTakenAirborneDamage)
        {
            if (Time.time - lastDamageTime < damageCooldown)
                return; // Still in cooldown

            lastDamageTime = Time.time;
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
            if (Time.time - lastDamageTime < damageCooldown)
                return; // Still in cooldown

            lastDamageTime = Time.time;
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

        // Play hit sound
        if (audioSource != null && hitSound != null)
            audioSource.PlayOneShot(hitSound);

        // Update UI hearts
        if (UIManager.instance != null)
            UIManager.instance.UpdateHearts(currentHP);

        // Disable movement/input
        isStunned = true;
        stunTimer = 0f; // Reset stun timer

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

        // Update hearts UI
        if (UIManager.instance != null)
            UIManager.instance.UpdateHearts(currentHP);

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

        // Save the new teleport position using SaveManager
        if (SaveManager.instance != null)
        {
            SaveManager.instance.SaveFloatData(SaveManager.TeleportX, teleportPosition.x);
            SaveManager.instance.SaveFloatData(SaveManager.TeleportY, teleportPosition.y);
            SaveManager.instance.SaveFloatData(SaveManager.TeleportZ, teleportPosition.z);
            SaveManager.instance.SaveData();
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a sphere at the teleport position for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(teleportPosition, 0.2f);
    }

}
