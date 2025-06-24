using UnityEngine;

public class BasicMovingPlatform : MonoBehaviour
{
    public enum RotationDirection { Clockwise, Counterclockwise }
    public enum MovementMode { Path, Random }

    [Header("Platform Settings")]
    [Range(0f, 50f)]
    public float moveSpeed = 2f; // How fast the platform moves

    [Range(0f, 360f)]
    public float rotationSpeed = 90f; // Degrees per second

    [Header("Enable Features")]
    public bool enableMovement = true;
    public bool enableRotation = true;

    [Header("Overload Settings")]
    public bool overload = false;

    [Header("Rotation Settings")]
    public RotationDirection rotationDirection = RotationDirection.Clockwise;

    [Header("Movement Settings")]
    public MovementMode movementMode = MovementMode.Path;

    [Tooltip("Set the positions (relative to the starting position) the platform will move between.")]
    public Vector2[] localMovePoints;

    [Tooltip("If using Random mode, this is the max distance from the start position.")]
    [Range(0f, 50f)]
    public float randomMoveRange = 5f;

    private Vector2 startPosition;
    private int currentTargetIndex = 0;
    private Vector2 randomTarget;

    // Store original speeds for overload toggle
    private float originalMoveSpeed;
    private float originalRotationSpeed;
    private bool overloadWasActive = false;

    void Start()
    {
        startPosition = transform.position;
        if (movementMode == MovementMode.Random)
        {
            PickRandomTarget();
        }
        // Initialize original speeds
        originalMoveSpeed = moveSpeed;
        originalRotationSpeed = rotationSpeed;
        overloadWasActive = overload;
    }

    void Update()
    {
        HandleOverload();

        if (enableMovement)
        {
            if (movementMode == MovementMode.Path && localMovePoints != null && localMovePoints.Length > 0)
            {
                Vector2 targetPosition = startPosition + localMovePoints[currentTargetIndex];
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                if (Vector2.Distance(transform.position, targetPosition) < 0.05f)
                {
                    currentTargetIndex = (currentTargetIndex + 1) % localMovePoints.Length;
                }
            }
            else if (movementMode == MovementMode.Random)
            {
                transform.position = Vector2.MoveTowards(transform.position, randomTarget, moveSpeed * Time.deltaTime);

                if (Vector2.Distance(transform.position, randomTarget) < 0.05f)
                {
                    PickRandomTarget();
                }
            }
        }

        if (enableRotation)
        {
            float directionMultiplier = rotationDirection == RotationDirection.Clockwise ? -1f : 1f;
            transform.Rotate(Vector3.forward, directionMultiplier * rotationSpeed * Time.deltaTime);
        }
    }

    void HandleOverload()
    {
        if (overload && !overloadWasActive)
        {
            // Store current values before overload
            originalMoveSpeed = moveSpeed;
            originalRotationSpeed = rotationSpeed;
            // Set to max values
            moveSpeed = 50f;
            rotationSpeed = 360f;
            overloadWasActive = true;
        }
        else if (!overload && overloadWasActive)
        {
            // Restore original values
            moveSpeed = originalMoveSpeed;
            rotationSpeed = originalRotationSpeed;
            overloadWasActive = false;
        }
    }

    void PickRandomTarget()
    {
        float offsetX = Random.Range(-randomMoveRange, randomMoveRange);
        float offsetY = Random.Range(-randomMoveRange, randomMoveRange);
        randomTarget = startPosition + new Vector2(offsetX, offsetY);
    }
}
