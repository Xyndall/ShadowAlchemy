using UnityEngine;

public class BasicAISimple : MonoBehaviour
{
    [Header("Patrol Settings")]
    [Tooltip("Points (in local coordinates) the AI will patrol between.")]
    public Vector3[] patrolPoints;
    [Tooltip("Movement speed while patrolling.")]
    public float patrolSpeed = 2f;

    [Header("Chase Settings")]
    [Tooltip("Movement speed while chasing the player.")]
    public float chaseSpeed = 4f;
    [Tooltip("How far the AI can see the player.")]
    public float sightRange = 5f;
    [Tooltip("Field of view angle in degrees.")]
    [Range(0f, 180f)]
    public float fieldOfView = 90f;

    [Header("Player Reference")]
    [Tooltip("Assign the player GameObject here.")]
    public Transform player;

    private int currentPatrolIndex = 0;
    private bool chasingPlayer = false;
    private SpriteRenderer spriteRenderer;
    private Vector2 facingDirection = Vector2.right; // Track facing direction

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player != null && IsPlayerInSight())
        {
            chasingPlayer = true;
            ChasePlayer();
        }
        else
        {
            chasingPlayer = false;
            Patrol();
        }
    }

    void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        Vector3 targetLocal = patrolPoints[currentPatrolIndex];
        Vector3 targetWorld = transform.parent != null
            ? transform.parent.TransformPoint(targetLocal)
            : transform.TransformPoint(targetLocal);

        Vector3 direction = targetWorld - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetWorld, patrolSpeed * Time.deltaTime);

        // Flip sprite and update facing direction
        if (spriteRenderer != null && direction.x != 0)
        {
            spriteRenderer.flipX = direction.x < 0;
            facingDirection = direction.x < 0 ? Vector2.left : Vector2.right;
        }

        if (Vector3.Distance(transform.position, targetWorld) < 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    void ChasePlayer()
    {
        Vector3 target = player.position;
        Vector3 direction = target - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, target, chaseSpeed * Time.deltaTime);

        // Flip sprite and update facing direction
        if (spriteRenderer != null && direction.x != 0)
        {
            spriteRenderer.flipX = direction.x < 0;
            facingDirection = direction.x < 0 ? Vector2.left : Vector2.right;
        }
    }

    bool IsPlayerInSight()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > sightRange)
            return false;

        // Use facingDirection for field of view
        float angle = Vector3.Angle(facingDirection, directionToPlayer.normalized);
        if (angle > fieldOfView * 0.5f)
            return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, sightRange);
        if (hit.collider != null && hit.collider.transform == player)
            return true;

        return false;
    }

    void OnDrawGizmosSelected()
    {
        // Draw patrol points
        Gizmos.color = Color.green;
        if (patrolPoints != null)
        {
            foreach (var pt in patrolPoints)
            {
                Vector3 worldPt = transform.parent != null
                    ? transform.parent.TransformPoint(pt)
                    : transform.TransformPoint(pt);
                Gizmos.DrawSphere(worldPt, 0.15f);
            }
        }

        // Draw sight range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        // Draw field of view using facingDirection
        Vector3 facing = Application.isPlaying ? (Vector3)facingDirection : transform.right;
        Vector3 leftBoundary = Quaternion.Euler(0, 0, -fieldOfView * 0.5f) * facing;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, fieldOfView * 0.5f) * facing;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * sightRange);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * sightRange);
    }
}