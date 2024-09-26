using UnityEngine;
using UnityEngine.InputSystem;

public class NewGrappleTest : MonoBehaviour
{
    [Header("Scripts:")]
    public GrappleRope grappleRope;
    [Header("Layer Settings:")]
    [SerializeField] private bool grappleToAll = false;
    [SerializeField] private int grappableLayerNumber = 9;// Layer mask to detect interactable objects
    [SerializeField] private int slingshotLayerNumber = 10;// Layer mask to detect interactable objects
    public LayerMask ignoreLayer;

    [Header("Main Camera")]
    public Camera m_camera;

    [Header("Transform Refrences:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Rotation:")]
    [Range(0, 360)][SerializeField] private float rotationSpeed = 4;

    [Header("Distance:")]
    

    [Header("Launching")]
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private LaunchType Launch_Type = LaunchType.Transform_Launch;
    [Range(0, 5)][SerializeField] private float launchSpeed = 5;

    [Header("No Launch To Point")]
    [SerializeField] private bool autoCongifureDistance = false;
    [SerializeField] private float targetDistance = 3;
    [SerializeField] private float targetFrequency = 3;


    [Header("Raycast Arc")]
    public float arcAngle = 45f;        // Total angle of the arc
    public int numberOfRaycasts = 10;   // Number of raycasts in the arc
    public bool hasMaxDistance = true; //Checks if has maxDistance
    public float maxDistance = 1;  // Max distance of each raycast
     

    private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch,
    }

    [Header("Component Refrences:")]
    public SpringJoint2D m_springJoint2D;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 DistanceVector;
    [HideInInspector] public bool isSlingshotting;

    public Rigidbody2D ballRigidbody;
    [HideInInspector]public bool validGrapplePoint = false;

    [Header("HUD")]
    public GameObject RotationDirImg;


    [Header("Other")]
    private Vector3 checkpointPos;
    private bool checkpointSet = false;
    public GameObject playerBase;
    public WallStick wallStick;
    PlayerInputActions playerInputActions;
    bool isHoldingGrapple;

    private void Start()
    {
        m_camera = Camera.main;
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
        ballRigidbody.gravityScale = 1;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Grapple.performed += Grapple_performed;
        playerInputActions.Player.Grapple.canceled += Grapple_released;
    }

    private void Update()
    {
        Debug.DrawRay(firePoint.position, gunPivot.transform.right * maxDistance);

        if (Input.GetKeyDown(KeyCode.R)) ReverseSpin();
        if (Input.GetKeyDown(KeyCode.T)) SetCheckpoint();
        if (Input.GetKeyDown(KeyCode.F)) RestartAtCheckpoint();

        if(isHoldingGrapple)
        {
            launchSpeed = 0.8f;
            RotateGun();
        }

        if (launchToPoint && grappleRope.isGrappling)
        {
            if (Launch_Type == LaunchType.Transform_Launch)
            {
                gunHolder.position = Vector3.Lerp(gunHolder.position, grapplePoint, Time.deltaTime * launchSpeed);
            }
        }

    }

    private void Grapple_released(InputAction.CallbackContext context)
    {
        
        if (context.canceled && !grappleRope.isGrappling && !grappleRope.GrappleRetracting && isHoldingGrapple)
        {
            if (!CastCenterRay()) SetGrapplePoint();

        }
        else if (context.canceled && grappleRope.isGrappling)
        {
            DisableGrapple();
        }
        isHoldingGrapple = false;
    }
    private void Grapple_performed(InputAction.CallbackContext context)
    {
        if (context.performed && !grappleRope.isGrappling && !grappleRope.GrappleRetracting)
        {
            isHoldingGrapple = true;
        }
    }



    public void DisableGrapple()
    {
        grappleRope.enabled = false;
        grappleRope.GrappleRetracting = false;
        grappleRope.isGrappling = false;
        m_springJoint2D.enabled = false;
        ballRigidbody.gravityScale = 1;
        validGrapplePoint = false;
        isSlingshotting = false;
    }

    public void ReverseSpin()
    {

        rotationSpeed = rotationSpeed * -1;
        RotationDirImg.transform.localScale = new Vector3(RotationDirImg.transform.localScale.x * -1, 1, 1);
    }

    void SetCheckpoint()
    {
        checkpointPos = transform.position;
        checkpointSet = true;  // Set a flag to indicate the checkpoint is set
        Debug.Log("Checkpoint set at: " + checkpointPos);
    }

    void RestartAtCheckpoint()
    {
        if (checkpointSet)  // Ensure the checkpoint has been set
        {
            playerBase.transform.position = checkpointPos;
            // Stop the Rigidbody's movement
            ballRigidbody.velocity = Vector2.zero;
            ballRigidbody.angularVelocity = 0f; // If your game involves rotation
            Debug.Log("Teleported to checkpoint: " + checkpointPos);
        }
        else
        {
            Debug.LogWarning("No checkpoint set! Press 'T' to set a checkpoint first.");
        }
    }

    void RotateGun()
    {
        float rotationMultiplier;
        if (Input.GetKey(KeyCode.LeftShift)) { rotationMultiplier = 2; }
        else { rotationMultiplier = 1; }


        // Calculate the rotation amount
        float rotationAmount = (rotationSpeed * rotationMultiplier) * Time.deltaTime;

        // Apply the rotation to the object
        gunPivot.transform.Rotate(Vector3.forward, rotationAmount);

    }

    bool CastCenterRay()
    {
        Vector2 origin = firePoint.position;
        Vector2 direction = transform.right;
        // Determine the raycast distance
        float raycastDistance = hasMaxDistance ? maxDistance : 100f;

        // Perform the raycast
        RaycastHit2D _hit = Physics2D.Raycast(origin, direction, raycastDistance, ~ignoreLayer);

        if (_hit.collider != null)
        {
            // If it hits a grappable object or grappleToAll is true, and within max distance
            if ((_hit.transform.gameObject.layer == grappableLayerNumber || grappleToAll) &&
                (Vector2.Distance(_hit.point, origin) <= maxDistance || !hasMaxDistance))
            {
                CalculateGrapplePosition(_hit.point, true);
                return true; // Return true if something was hit
            }
            else if (_hit.transform.gameObject.layer == slingshotLayerNumber)
            {
                Slingshot();
                CalculateGrapplePosition(_hit.point, true);
                return true; // Return true if something was hit
            }
            else
            {
                CalculateGrapplePosition(_hit.point, false);
                return true; // return false if something was hit but not grappabble
            }

        }
        else
        {
            return false; // Return false if nothing was hit
        }
        
    }

    void Slingshot()
    {
        isSlingshotting = true;
        launchSpeed = 2;
    }


    void SetGrapplePoint()
    {
        Vector2 origin = firePoint.position;
        Vector2 forward = transform.right; // For 2D, right is the "forward" direction
        float halfArcAngle = arcAngle / 2f;

        // Determine the raycast distance
        float raycastDistance = hasMaxDistance ? maxDistance : 100f;



        // Loop to cast multiple raycasts
        for (int i = 0; i <= numberOfRaycasts; i++)
        {
            // Calculate the current angle for this raycast
            float currentAngle = -halfArcAngle + (i * (arcAngle / numberOfRaycasts));

            // Calculate the direction based on the current angle
            Vector2 direction = Quaternion.Euler(0, 0, currentAngle) * forward;

            // Perform the raycast
            RaycastHit2D _hit = Physics2D.Raycast(origin, direction, raycastDistance, ~ignoreLayer);

            // Check if the raycast hit something
            if (_hit.collider != null)
            {
                // If it hits a grappable object or grappleToAll is true, and within max distance
                if ((_hit.transform.gameObject.layer == grappableLayerNumber || grappleToAll) &&
                    (Vector2.Distance(_hit.point, origin) <= maxDistance || !hasMaxDistance))
                {
                    CalculateGrapplePosition(_hit.point, true);
                    Debug.DrawRay(origin, direction * _hit.distance, Color.green, 10);
                }
                else if (_hit.transform.gameObject.layer == slingshotLayerNumber)
                {
                    Slingshot();
                    CalculateGrapplePosition(_hit.point, true);
                    Debug.DrawRay(origin, direction * _hit.distance, Color.green, 10);
                }
                else
                {
                    // Check if this is the last raycast
                    if (i == numberOfRaycasts && !validGrapplePoint)
                    {
                        CalculateGrapplePosition(_hit.point, false);
                    }
                    Debug.DrawRay(origin, direction * maxDistance, Color.red, 10);

                }

            }
            else
            {
                // Check if this is the last raycast
                if (i == numberOfRaycasts && !validGrapplePoint)
                {
                    CalculateGrapplePosition((Vector2)firePoint.position + (Vector2)(gunPivot.transform.right * raycastDistance), false);
                }
                Debug.DrawRay(origin, direction * maxDistance, Color.red, 10);
            }
            
        }

    }

    void CalculateGrapplePosition(Vector2 hitPos, bool isValid)
    {
        // Set the grapple point to the hit point
        grapplePoint = hitPos;
        validGrapplePoint = isValid;
        
        // Calculate the distance vector and enable the grapple rope
        DistanceVector = grapplePoint - (Vector2)gunPivot.position;
        grappleRope.enabled = true;
    }


    public void Grapple()
    {
        // Only proceed if the grapple point is valid
        if (!validGrapplePoint)
        {
            grappleRope.GrappleRetracting = true;
            return;
        }
        grappleRope.isGrappling = true;
        wallStick.UnstickFromWall();

        if (!launchToPoint && !autoCongifureDistance)
        {
            m_springJoint2D.distance = targetDistance;
            m_springJoint2D.frequency = targetFrequency;
        }

        if (!launchToPoint)
        {
            if (autoCongifureDistance)
            {
                m_springJoint2D.autoConfigureDistance = true;
                m_springJoint2D.frequency = 0;
            }
            m_springJoint2D.connectedAnchor = grapplePoint;
            m_springJoint2D.enabled = true;
        }

        else
        {
            if (Launch_Type == LaunchType.Transform_Launch)
            {
                ballRigidbody.gravityScale = 0;
                ballRigidbody.velocity = Vector2.zero;
            }
            if (Launch_Type == LaunchType.Physics_Launch)
            {
                m_springJoint2D.connectedAnchor = grapplePoint;
                m_springJoint2D.distance = 0;
                m_springJoint2D.frequency = launchSpeed;
                m_springJoint2D.enabled = true;
            }
        }

    }

    private void OnDrawGizmos()
    {
        if (hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistance);
        }
    }
}
