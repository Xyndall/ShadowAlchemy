using UnityEngine;

public class GrappleRope : MonoBehaviour
{
    [Header("General refrences:")]
    public GrapplingGun grapplingGun;
    public NewGrappleTest newGrapplingGun;
    [SerializeField] LineRenderer m_lineRenderer;

    [Header("General Settings:")]
    [SerializeField] private int percision = 20;
    [Range(0, 100)][SerializeField] private float straightenLineSpeed = 4;

    [Header("Animation:")]
    public AnimationCurve ropeAnimationCurve;
    [SerializeField] [Range(0.01f, 4)] private float WaveSize = 20;
    float waveSize;

    [Header("Rope Speed:")]
    public AnimationCurve ropeLaunchSpeedCurve;
    [SerializeField] [Range(1, 50)] private float ropeLaunchSpeedMultiplayer = 4;

    float moveTime = 0;

    [SerializeField]public bool isGrappling = false;
    [SerializeField]public bool GrappleRetracting = false;
    
    bool drawLine = true;
    bool straightLine = true;

    bool hasGrapplingGun;
    bool hasNewGrapplingGun;

    private void Awake()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
        m_lineRenderer.enabled = false;
        m_lineRenderer.positionCount = percision;
        waveSize = WaveSize;
    }
    private void Start()
    {
        if (grapplingGun != null) hasGrapplingGun = true;
        if (newGrapplingGun != null) hasNewGrapplingGun = true;
    }

    private void OnEnable()
    {
        moveTime = 0;
        m_lineRenderer.enabled = true;
        m_lineRenderer.positionCount = percision;
        waveSize = WaveSize;
        straightLine = false;
        LinePointToFirePoint();
    }

    private void OnDisable()
    {
        m_lineRenderer.enabled = false;
        isGrappling = false;
    }

    void LinePointToFirePoint()
    {
        for (int i = 0; i < percision; i++)
        {
            if (hasGrapplingGun) m_lineRenderer.SetPosition(i, grapplingGun.firePoint.position);
            if (hasNewGrapplingGun) m_lineRenderer.SetPosition(i, newGrapplingGun.firePoint.position);

        }
    }

    void Update()
    {
        

        if (drawLine)
        {
            DrawRope();
        }
    }

    void DrawRope()
    {
        if (!straightLine) 
        {
            if (hasGrapplingGun)
            {
                if (m_lineRenderer.GetPosition(percision - 1).x != grapplingGun.grapplePoint.x)
                {
                    DrawRopeWaves();
                }
                else
                {
                    straightLine = true;
                }
            }
            if (hasNewGrapplingGun)
            {
                if (m_lineRenderer.GetPosition(percision - 1).x != newGrapplingGun.grapplePoint.x)
                {
                    DrawRopeWaves();
                }
                else
                {
                    straightLine = true;
                }
            }
            
        }
        else 
        {
            if (!isGrappling) 
            {
                if(hasGrapplingGun) grapplingGun.Grapple();
                if(newGrapplingGun != null) newGrapplingGun.Grapple();
                
            }
            if (waveSize > 0)
            {
                waveSize -= Time.deltaTime * straightenLineSpeed;
                DrawRopeWaves();
            }
            else 
            {
                waveSize = 0;
                DrawRopeNoWaves();
                if(grapplingGun != null && grapplingGun.validGrapplePoint == false) RetractRopeWaves();
                if(newGrapplingGun != null && newGrapplingGun.validGrapplePoint == false) RetractRopeWaves();
                
            }
        }
    }

    void DrawRopeWaves() 
    {
        moveTime += Time.deltaTime;
        for (int i = 0; i < percision; i++)
        {
            if (hasGrapplingGun)
            {

                float delta = (float)i / ((float)percision - 1f);
                Vector2 offset = Vector2.Perpendicular(grapplingGun.DistanceVector).normalized * ropeAnimationCurve.Evaluate(delta) * waveSize;
                Vector2 targetPosition = Vector2.Lerp(grapplingGun.firePoint.position, grapplingGun.grapplePoint, delta) + offset;
                Vector2 currentPosition = Vector2.Lerp(grapplingGun.firePoint.position, targetPosition, ropeLaunchSpeedCurve.Evaluate(moveTime) * ropeLaunchSpeedMultiplayer);

                m_lineRenderer.SetPosition(i, currentPosition);
            }
            if (hasNewGrapplingGun)
            {

                float delta = (float)i / ((float)percision - 1f);
                Vector2 offset = Vector2.Perpendicular(newGrapplingGun.DistanceVector).normalized * ropeAnimationCurve.Evaluate(delta) * waveSize;
                Vector2 targetPosition = Vector2.Lerp(newGrapplingGun.firePoint.position, newGrapplingGun.grapplePoint, delta) + offset;
                Vector2 currentPosition = Vector2.Lerp(newGrapplingGun.firePoint.position, targetPosition, ropeLaunchSpeedCurve.Evaluate(moveTime) * ropeLaunchSpeedMultiplayer);

                m_lineRenderer.SetPosition(i, currentPosition);
            }


        }
       
    }

    void RetractRopeWaves()
    {
        
        moveTime -= Time.deltaTime;
        if (m_lineRenderer.positionCount != percision)
        {
            m_lineRenderer.positionCount = percision;  // Ensure position count is correct
        }

        for (int i = 0; i < percision; i++)
        {
            float delta = (float)i / ((float)percision - 1f);
            Vector2 offset = Vector2.zero;
            Vector2 targetPosition;
            Vector2 currentPosition;

            if (hasGrapplingGun)
            {
                offset = Vector2.Perpendicular(grapplingGun.DistanceVector).normalized * ropeAnimationCurve.Evaluate(delta) * waveSize;

                // Always interpolate from grapplePoint to firePoint
                targetPosition = Vector2.Lerp(grapplingGun.grapplePoint, grapplingGun.firePoint.position, delta) + offset;
                currentPosition = Vector2.Lerp(grapplingGun.grapplePoint, targetPosition, ropeLaunchSpeedCurve.Evaluate(moveTime) * ropeLaunchSpeedMultiplayer);

                m_lineRenderer.SetPosition(i, currentPosition);
            }
            else if (hasNewGrapplingGun)
            {
                offset = Vector2.Perpendicular(newGrapplingGun.DistanceVector).normalized * ropeAnimationCurve.Evaluate(delta) * waveSize;

                // Always interpolate from grapplePoint to firePoint
                targetPosition = Vector2.Lerp(newGrapplingGun.grapplePoint, newGrapplingGun.firePoint.position, delta) + offset;
                currentPosition = Vector2.Lerp(newGrapplingGun.grapplePoint, targetPosition, ropeLaunchSpeedCurve.Evaluate(moveTime) * ropeLaunchSpeedMultiplayer);

                m_lineRenderer.SetPosition(i, currentPosition);
            }
        }
        if (moveTime <= 0f)
        {
            moveTime = 0f;
            if (grapplingGun != null) grapplingGun.DisableGrapple();
            if (newGrapplingGun != null) newGrapplingGun.DisableGrapple();
        }
    }



    void DrawRopeNoWaves() 
    {
        if (hasGrapplingGun)
        {
            m_lineRenderer.positionCount = 2;
            m_lineRenderer.SetPosition(0, grapplingGun.grapplePoint);
            m_lineRenderer.SetPosition(1, grapplingGun.firePoint.position);
        }

        if (hasNewGrapplingGun)
        {
            m_lineRenderer.positionCount = 2;
            m_lineRenderer.SetPosition(0, newGrapplingGun.grapplePoint);
            m_lineRenderer.SetPosition(1, newGrapplingGun.firePoint.position);
        }
        if (hasNewGrapplingGun)
        {
            if (newGrapplingGun.isSlingshotting)
            {
                newGrapplingGun.DisableGrapple();
            }
        }

    }

}
