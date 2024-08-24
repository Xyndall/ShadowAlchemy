using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawArcRaycastTests : MonoBehaviour
{

    public float arcAngle = 45f;        // Total angle of the arc
    public int numberOfRaycasts = 10;   // Number of raycasts in the arc
    public float maxDistance = 100f;    // Max distance of each raycast
    public LayerMask interactionLayer;  // Layer mask to detect interactable objects

    void Update()
    {
        DrawArcRaycasts();
    }

    void DrawArcRaycasts()
    {
        Vector2 origin = transform.position;
        Vector2 forward = transform.right; // For 2D, right is the "forward" direction
        float halfArcAngle = arcAngle / 2f;

        // Loop to cast multiple raycasts
        for (int i = 0; i <= numberOfRaycasts; i++)
        {
            // Calculate the current angle for this raycast
            float currentAngle = -halfArcAngle + (i * (arcAngle / numberOfRaycasts));

            // Calculate the direction based on the current angle
            Vector2 direction = Quaternion.Euler(0, 0, currentAngle) * forward;

            // Cast the ray
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance, interactionLayer);

            if (hit.collider != null)
            {
                Debug.DrawRay(origin, direction * hit.distance, Color.green);
                // Interact with the object
                // hit.collider.GetComponent<InteractableObject>()?.Interact();
            }
            else
            {
                Debug.DrawRay(origin, direction * maxDistance, Color.red);
            }
        }
    }
}


