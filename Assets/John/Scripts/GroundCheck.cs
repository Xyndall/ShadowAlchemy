using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public float distanceCheck;
    public float offset;
    public float jumpGrace;
    public float jumpGraceBase;
    public bool groundCheck;
    public bool on;
    public Vector3 surfacePosition;
    public Vector3 overlapBoxSize;
    public LayerMask toDetect;
    Vector2 point;
    void Update()
    {
        point = transform.position + Vector3.down * offset;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distanceCheck, toDetect);


        if (hit)
        {
            jumpGrace = jumpGraceBase;
            groundCheck = true;
            surfacePosition = Physics2D.ClosestPoint(new Vector2(transform.position.x, transform.position.y * offset), hit.collider);
        }
        else
        {
            groundCheck = false;
            jumpGrace -= Time.deltaTime;
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (on)
        {
            Gizmos.DrawCube(point, overlapBoxSize);

        }
    }
}
