using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;


public class NewGrappleTest : MonoBehaviour
{
    public static NewGrappleTest instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
        [Header("Scripts:")]
    public GrappleRope grappleRope;
    public GrappleAudioContoller GrappleAudio;

    [Header("Layer Settings:")]
    [SerializeField] private bool grappleToAll = false;
    [SerializeField] private int unGrappableLayerNumber = 8;// Layer mask to detect unInteractable objects
    [SerializeField] private int grappableLayerNumber = 9;// Layer mask to detect interactable objects
    [SerializeField] private int slingshotLayerNumber = 10;// Layer mask to detect interactable objects
    [SerializeField] private int StickyLayerNumber = 11;// Layer mask to detect interactable objects
    public LayerMask ignoreLayer;

    [Header("Main Camera")]
    public Camera m_camera;

    [Header("Transform Refrences:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Rotation:")]
    //used for easy grapple, always stays true.
    [SerializeField] private bool rotateOverTime = true;
    [Range(0, 360)][SerializeField] private float rotationSpeed = 4;

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


    [Header("Other")]
    private Vector3 checkpointPos;
    private bool checkpointSet = false;
    public GameObject playerBase;
    public WallStick wallStick;
    PlayerInputActions playerInputActions;
    bool isHoldingGrapple;
    bool isHoldingButton;
    public string SurfaceTypeHit;
    public PlayerAnimationController pAnimaor;
    public bool EasyModeGrapple;
    public int GrappleAmountMissed;
    [HideInInspector] public Transform grappledObject; // The object being grappled
    private Vector2 grappledLocalPoint; // The local point on the object where the grapple attached

    private Vector2 aimInput;
    private bool isUsingGamepad = false;

    //Surface types const strings
    [HideInInspector] public const string AirSurface = "Air";
    [HideInInspector] public const string MetalSurface = "Metal";
    [HideInInspector] public const string GroundSurface = "Ground";
    [HideInInspector] public const string StickySurface = "Sticky";
    [HideInInspector] public const string SlingshotSurface = "Slingshot";
    

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
        playerInputActions.Player.Aim.performed += ctx =>
        {
            aimInput = ctx.ReadValue<Vector2>();
            if (ctx.control.device is Gamepad && aimInput.sqrMagnitude > 0.1f)
                isUsingGamepad = true;
        };
        playerInputActions.Player.Aim.canceled += ctx => aimInput = Vector2.zero;

    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>().sqrMagnitude > 0.1f)
            isUsingGamepad = false;
    }

    private void Update()
    {
        bool isGamePaused = UIManager.instance.gameIsPaused;
        if (isGamePaused) playerInputActions.Player.Disable();
        else playerInputActions.Player.Enable();

        Debug.DrawRay(firePoint.position, gunPivot.transform.right * maxDistance);

        //if (Input.GetKeyDown(KeyCode.R)) ReverseSpin();
        //if (Input.GetKeyDown(KeyCode.T)) SetCheckpoint();
        //if (Input.GetKeyDown(KeyCode.F)) RestartAtCheckpoint();
        //if(Input.GetKeyDown(KeyCode.H)) SteamAchievements.ResetAchievements();

        //Checks if easymode is on or not.
        if (!EasyModeGrapple)
        {
            if (isHoldingButton)
            {
                    isHoldingGrapple = true;
                    pAnimaor.SetAHoldingButton(true);
                    launchSpeed = 0.8f;
                    RotateGun();
            }
            
        }
        else if (EasyModeGrapple)
        {

            if (isUsingGamepad)
            {
                Vector3 lookDirection = (Vector3)aimInput.normalized + gunPivot.position;
                EasyRotateGun(lookDirection, true);
            }
            else
            {
                EasyRotateGun(m_camera.ScreenToWorldPoint(Input.mousePosition), false);
            }
            
            
            if (isHoldingButton)
            {
                    isHoldingGrapple = true;
                    pAnimaor.SetAHoldingButton(true);
                    launchSpeed = 0.8f;
            }
        }

        if (grappleRope.isGrappling && grappledObject != null)
        {
            // Update the grapple point to follow the moving object
            Vector2 newGrapplePoint = grappledObject.TransformPoint(grappledLocalPoint);
            grapplePoint = newGrapplePoint;

            // If using SpringJoint2D, update its anchor
            if (m_springJoint2D.enabled)
                m_springJoint2D.connectedAnchor = newGrapplePoint;
        }

        if (launchToPoint && grappleRope.isGrappling)
        {
            if (Launch_Type == LaunchType.Transform_Launch)
            {
                gunHolder.position = Vector3.Lerp(gunHolder.position, grapplePoint, Time.deltaTime * launchSpeed);
            }
        }

        if (grappleRope.GrappleRetracting)
        {
            GrappleAudio.PlayRetractSound();
        }
        else
        {
            GrappleAudio.hasPlayedRetractSound = false;
        }
    }

    private void Grapple_released(InputAction.CallbackContext context)
    {
        isHoldingButton = false;
        GrappleAudio.PlayGrappleSound();
        
        if (context.canceled && !grappleRope.isGrappling && !grappleRope.GrappleRetracting && isHoldingGrapple)
        {
            
            if (!CastCenterRay()) SetGrapplePoint();
            pAnimaor.SetAHoldingButton(false);
        }
        else if (context.canceled && grappleRope.isGrappling)
        {
            DisableGrapple();
            pAnimaor.SetAHoldingButton(false);
            
        }
        isHoldingGrapple = false;
    }
    private void Grapple_performed(InputAction.CallbackContext context)
    {
         
         if (!grappleRope.isGrappling && !grappleRope.GrappleRetracting)
         {
            isHoldingButton = true;
            GrappleAudio.PlayReadyingSound();
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
        pAnimaor.SetIsGrappling(false);
        grappledObject = null;
    }

    public void ReverseSpin()
    {
        rotationSpeed *= -1;
        
        
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
            ballRigidbody.angularVelocity = 0f; 
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
    void EasyRotateGun(Vector3 lookPoint, bool allowRotationOverTime)
    {
        Vector3 distanceVector = lookPoint - gunPivot.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        if (rotateOverTime && allowRotationOverTime)
        {
            Quaternion startRotation = gunPivot.rotation;
            gunPivot.rotation = Quaternion.Lerp(startRotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
        }
        else
            gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

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
                // Store the object and local point
                grappledObject = _hit.transform;
                grappledLocalPoint = grappledObject.InverseTransformPoint(_hit.point);
                CalculateGrapplePosition(_hit.point, true, GroundSurface);
                return true; // Return true if something was hit
            }
            else if (_hit.transform.gameObject.layer == slingshotLayerNumber)
            {
                Slingshot();
                CalculateGrapplePosition(_hit.point, true, SlingshotSurface);
                return true; // Return true if something was hit
            }
            else if(_hit.transform.gameObject.layer == unGrappableLayerNumber)
            {
                CalculateGrapplePosition(_hit.point, false, MetalSurface);
                return true; // Return true if something was hit nut not grappabble
            }
            else if(_hit.transform.gameObject.layer == StickyLayerNumber)
            {
                CalculateGrapplePosition(_hit.point, false, MetalSurface);
                return true; // Return true if something was hit nut not grappabble
            }
            else
            {
                CalculateGrapplePosition(_hit.point, false, AirSurface);
                return true; // return true if something was hit but not grappabble
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
                    // Store the object and local point
                    grappledObject = _hit.transform;
                    grappledLocalPoint = grappledObject.InverseTransformPoint(_hit.point);
                    CalculateGrapplePosition(_hit.point, true, GroundSurface);
                    Debug.DrawRay(origin, direction * _hit.distance, Color.green, 10);
                }
                else if (_hit.transform.gameObject.layer == slingshotLayerNumber)
                {
                    Slingshot();
                    CalculateGrapplePosition(_hit.point, true, SlingshotSurface);
                }
                else if (_hit.transform.gameObject.layer == unGrappableLayerNumber)
                {
                    CalculateGrapplePosition(_hit.point, false, MetalSurface);
                }
                else if (_hit.transform.gameObject.layer == StickyLayerNumber)
                {
                    CalculateGrapplePosition(_hit.point, false, MetalSurface);
                }
                else
                {
                    // Check if this is the last raycast
                    if (i == numberOfRaycasts && !validGrapplePoint)
                    {
                        CalculateGrapplePosition(_hit.point, false, AirSurface);
                    }
                    Debug.DrawRay(origin, direction * maxDistance, Color.red, 10);

                }

            }
            else
            {
                // Check if this is the last raycast
                if (i == numberOfRaycasts && !validGrapplePoint)
                {
                    CalculateGrapplePosition((Vector2)firePoint.position + (Vector2)(gunPivot.transform.right * raycastDistance), false, "Air");
                }
                Debug.DrawRay(origin, direction * maxDistance, Color.red, 10);
            }
            
        }

    }

    void CalculateGrapplePosition(Vector2 hitPos, bool isValid, string surfaceHit)
    {
        if (isValid)
        {
            pAnimaor.SetIsGrappling(true);
        }
        else 
        {
            GrappleAmountMissed++;
            SteamAchievements.UnlockAchievement("Ach_GrappleMiss");
            Debug.Log("Grapple Missed: " + GrappleAmountMissed);
        }

        // Set the grapple point to the hit point
        SurfaceTypeHit = surfaceHit;
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
            GrappleAudio.PlayHitSound();
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
