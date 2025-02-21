using UnityEngine;

public class GrappleMovingPlatformHandler : MonoBehaviour
{
    private Transform grappledPlatform;
    private Vector3 grapplePointOffset;
    private Vector3 grapplePoint;

    private SpringJoint2D springJoint;
    private GrappleRope grappleRope;

    public void Initialize(SpringJoint2D joint, GrappleRope rope)
    {
        springJoint = joint;
        grappleRope = rope;
    }

    public void SetGrapplePoint(Vector3 point, RaycastHit2D hit)
    {
        grapplePoint = point;

        // Store platform reference and offset if hit a moving platform
        if (hit.collider.CompareTag("MovingPlatform"))
        {
            grappledPlatform = hit.collider.transform;
            grapplePointOffset = grapplePoint - grappledPlatform.position;
        }

        // Activate spring joint
        springJoint.connectedAnchor = grapplePoint;
        springJoint.enabled = true;
        springJoint.autoConfigureDistance = false;
        springJoint.distance = Vector2.Distance(transform.position, grapplePoint);
        springJoint.dampingRatio = 0.5f;
        springJoint.frequency = 1.5f;

        if (grappleRope != null)
        {
            grappleRope.enabled = true; // Enable Grapple Rope visuals
        }
    }

    public void UpdateGrapplePoint()
    {
        if (grappledPlatform != null)
        {
            grapplePoint = grappledPlatform.position + grapplePointOffset;
            springJoint.connectedAnchor = grapplePoint;
        }
    }

    public void ResetGrapple()
    {
        grappledPlatform = null;
        grapplePointOffset = Vector3.zero;
    }
}
