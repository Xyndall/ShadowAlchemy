using UnityEngine;

public class BasicMovingPlatform : MonoBehaviour
{
    public float moveRange = 3f;      // How far from the start position the platform can move
    public float moveSpeed = 2f;      // How fast the platform moves
    public float rotationSpeed = 90f; // Degrees per second

    private Vector2 startPosition;
    private Vector2 targetPosition;

    void Start()
    {
        startPosition = transform.position;
        PickNewTarget();
    }

    void Update()
    {
        // Move towards the target position
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Rotate the platform
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        // If reached the target, pick a new one
        if (Vector2.Distance(transform.position, targetPosition) < 0.05f)
        {
            PickNewTarget();
        }
    }

    void PickNewTarget()
    {
        float offsetX = Random.Range(-moveRange, moveRange);
        float offsetY = Random.Range(-moveRange, moveRange);
        targetPosition = startPosition + new Vector2(offsetX, offsetY);
    }
}
