using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float fireRate = 1f;

    [Header("Sprite Settings")]
    public Sprite frontactivatedSprite;
    public Sprite backactivatedSprite;
    public float resetSpriteDelay = 0.2f; // Time before sprite resets

    [Header("Trap Timing")]
    public float shootDelay = 0.5f; // Delay before shooting, adjustable in Inspector

    public SpriteRenderer frontspriteRenderer;
    public SpriteRenderer backspriteRenderer;
    private Sprite frontdefaultSprite;
    private Sprite backdefaultSprite;
    private float fireTimer = 0f;
    private bool isFiring = false;

    private bool isActive = false; // Controlled externally

    void Start()
    {
        if (frontspriteRenderer != null)
            frontdefaultSprite = frontspriteRenderer.sprite;
        if (backspriteRenderer != null)
            backdefaultSprite = backspriteRenderer.sprite;
    }

    void Update()
    {
        if (!isActive)
            return;

        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate && !isFiring)
        {
            StartCoroutine(FireWithDelay());
            fireTimer = 0f;
        }
    }

    private IEnumerator FireWithDelay()
    {
        isFiring = true;
        yield return new WaitForSeconds(shootDelay);
        ActivateTrap();
        isFiring = false;
    }

    public void ActivateTrap()
    {
        FireProjectile();
        SwapSprite();
        StartCoroutine(ResetSpriteAfterDelay());
    }

    private void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        projectile.transform.parent = transform;
        if (rb != null)
        {
            rb.velocity = -transform.right * projectileSpeed;
        }
    }

    private void SwapSprite()
    {
        if (frontspriteRenderer != null && frontactivatedSprite != null)
        {
            frontspriteRenderer.sprite = frontactivatedSprite;
        }
        if (backspriteRenderer != null && backactivatedSprite != null)
        {
            backspriteRenderer.sprite = backactivatedSprite;
        }
    }

    private IEnumerator ResetSpriteAfterDelay()
    {
        yield return new WaitForSeconds(resetSpriteDelay);
        if (frontspriteRenderer != null && frontdefaultSprite != null)
        {
            frontspriteRenderer.sprite = frontdefaultSprite;
        }
        yield return new WaitForSeconds(resetSpriteDelay);
        if (backspriteRenderer != null && backdefaultSprite != null)
        {
            backspriteRenderer.sprite = backdefaultSprite;
        }
    }

    // Called by trigger area
    public void EnableTrap()
    {
        isActive = true;
        fireTimer = fireRate; // Optional: fire immediately on activation
    }

    public void DisableTrap()
    {
        isActive = false;
        isFiring = false;
    }
}
