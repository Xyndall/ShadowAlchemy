using UnityEngine;

public class BasicMovingPlatform : MonoBehaviour
{
    public enum RotationDirection { Clockwise, Counterclockwise }
    public enum MovementMode { Path, Random }
    public enum RotationMode { Continuous, Swing }

    [Header("Platform Settings")]
    [Range(0f, 50f)]
    public float moveSpeed = 2f;

    [Range(0f, 360f)]
    public float rotationSpeed = 90f;

    [Header("Enable Features")]
    public bool enableMovement = true;
    public bool enableRotation = true;

    [Header("Overload Settings")]
    public bool overload = false;

    [Header("Rotation Settings")]
    public RotationDirection rotationDirection = RotationDirection.Clockwise;
    [Tooltip("Set to 'Swing' for back-and-forth swinging, or 'Continuous' for constant rotation.")]
    public RotationMode rotationMode = RotationMode.Continuous;

    [Header("Swing Settings")]
    [Tooltip("Total swing angle in degrees (e.g., 60 means ±30 from center).")]
    [Range(0f, 180f)]
    public float swingAngle = 60f;
    [Tooltip("How fast to swing (cycles per second).")]
    [Range(0.01f, 10f)]
    public float swingSpeed = 1f;
    [Tooltip("Local offset from the object's center for the swing pivot.")]
    public Vector2 swingPivotOffset = Vector2.zero;

    [Header("Movement Settings")]
    public MovementMode movementMode = MovementMode.Path;

    [Tooltip("Set the positions (in local coordinates, relative to parent) the platform will move between.")]
    public Vector3[] localMovePoints;

    [Tooltip("If using Random mode, this is the max distance from the start position.")]
    [Range(0f, 50f)]
    public float randomMoveRange = 5f;

    [Header("Path Movement Options")]
    public bool pingPongPath = false;

    private Vector2 startPosition;
    private int currentTargetIndex = 0;
    private Vector2 randomTarget;

    private float originalMoveSpeed;
    private float originalRotationSpeed;
    private bool overloadWasActive = false;

    // Unified rotation state
    private Vector3 initialPivotOffset;
    private Quaternion initialPivotRotation;
    private float swingTime = 0f;

    // New variable to control path direction for ping-pong effect
    private int pathDirection = 1;

    void Start()
    {
        startPosition = transform.position;
        if (movementMode == MovementMode.Random)
        {
            PickRandomTarget();
        }
        originalMoveSpeed = moveSpeed;
        originalRotationSpeed = rotationSpeed;
        overloadWasActive = overload;

        // Store the initial offset and rotation for both rotation modes
        Vector3 pivot = transform.TransformPoint(swingPivotOffset);
        initialPivotOffset = transform.position - pivot;
        initialPivotRotation = transform.rotation;
    }

    void FixedUpdate()
    {
        HandleOverload();

        if (enableMovement)
        {
            if (movementMode == MovementMode.Path && localMovePoints != null && localMovePoints.Length > 0)
            {
                Vector3 targetPosition = localMovePoints[currentTargetIndex];
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.localPosition, targetPosition) < 0.05f)
                {
                    if (pingPongPath)
                    {
                        // Reverse direction at ends
                        if ((currentTargetIndex == localMovePoints.Length - 1 && pathDirection == 1) ||
                            (currentTargetIndex == 0 && pathDirection == -1))
                        {
                            pathDirection *= -1;
                        }
                        currentTargetIndex += pathDirection;
                    }
                    else
                    {
                        currentTargetIndex = (currentTargetIndex + 1) % localMovePoints.Length;
                    }
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
            Vector3 pivot = transform.TransformPoint(swingPivotOffset);

            if (rotationMode == RotationMode.Continuous)
            {
                float directionMultiplier = rotationDirection == RotationDirection.Clockwise ? -1f : 1f;
                float angle = directionMultiplier * rotationSpeed * Time.time;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                Vector3 newOffset = rotation * initialPivotOffset;

                transform.position = pivot + newOffset;
                transform.rotation = rotation * initialPivotRotation;
            }
            else if (rotationMode == RotationMode.Swing)
            {
                swingTime += Time.deltaTime * swingSpeed * 2f * Mathf.PI; // radians per second
                float halfAngle = swingAngle * 0.5f;
                float angle = Mathf.Sin(swingTime) * halfAngle;
                Quaternion swingRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                Vector3 newOffset = swingRotation * initialPivotOffset;

                transform.position = pivot + newOffset;
                transform.rotation = swingRotation * initialPivotRotation;
            }
        }
    }

    void HandleOverload()
    {
        if (overload && !overloadWasActive)
        {
            originalMoveSpeed = moveSpeed;
            originalRotationSpeed = rotationSpeed;
            moveSpeed = 50f;
            rotationSpeed = 360f;
            overloadWasActive = true;
        }
        else if (!overload && overloadWasActive)
        {
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


    public Vector3 GetPlatformEndWorldPosition(Vector3 localEndOffset)
    {
        // Calculate the pivot in world space
        Vector3 pivot = transform.TransformPoint(swingPivotOffset);

        // Calculate the current rotation (matches your FixedUpdate logic)
        Quaternion rotation = transform.rotation;

        // The end offset is relative to the platform's local space
        // So, rotate it and add to the current position
        return transform.TransformPoint(localEndOffset);
    }

    void OnDrawGizmosSelected()
    {
        // Calculate the world position of the pivot
        Vector3 pivotWorld = transform.TransformPoint(swingPivotOffset);

        // Draw a yellow sphere at the pivot point
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pivotWorld, 0.15f);

        // Draw a line from the platform's center to the pivot
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, pivotWorld);

        // Optionally, draw the swing arc
        if (rotationMode == RotationMode.Swing)
        {
            float halfAngle = swingAngle * 0.5f;
            Vector3 startDir = Quaternion.Euler(0, 0, -halfAngle) * (transform.position - pivotWorld);
            Vector3 endDir = Quaternion.Euler(0, 0, halfAngle) * (transform.position - pivotWorld);
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(pivotWorld, pivotWorld + startDir);
            Gizmos.DrawLine(pivotWorld, pivotWorld + endDir);
        }
    }

}
