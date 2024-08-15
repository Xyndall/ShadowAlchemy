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
    
    bool drawLine = true;
    bool straightLine = true;
    

    private void Awake()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
        m_lineRenderer.enabled = false;
        m_lineRenderer.positionCount = percision;
        waveSize = WaveSize;
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
            if (grapplingGun != null) m_lineRenderer.SetPosition(i, grapplingGun.firePoint.position);
            if (newGrapplingGun != null) m_lineRenderer.SetPosition(i, newGrapplingGun.firePoint.position);

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
            if (grapplingGun != null)
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
            if (newGrapplingGun != null)
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
                if(grapplingGun != null) grapplingGun.Grapple();
                if(newGrapplingGun != null) newGrapplingGun.Grapple();
                isGrappling = true;
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
                if(newGrapplingGun.validGrapplePoint == false)
                {
                    RetractRopeWaves();
                }
            }
        }
    }

    void DrawRopeWaves() 
    {
        moveTime += Time.deltaTime;
        for (int i = 0; i < percision; i++)
        {
            if (grapplingGun != null)
            {

                float delta = (float)i / ((float)percision - 1f);
                Vector2 offset = Vector2.Perpendicular(grapplingGun.DistanceVector).normalized * ropeAnimationCurve.Evaluate(delta) * waveSize;
                Vector2 targetPosition = Vector2.Lerp(grapplingGun.firePoint.position, grapplingGun.grapplePoint, delta) + offset;
                Vector2 currentPosition = Vector2.Lerp(grapplingGun.firePoint.position, targetPosition, ropeLaunchSpeedCurve.Evaluate(moveTime) * ropeLaunchSpeedMultiplayer);

                m_lineRenderer.SetPosition(i, currentPosition);
            }
            if (newGrapplingGun != null)
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

            if (grapplingGun != null)
            {
                offset = Vector2.Perpendicular(grapplingGun.DistanceVector).normalized * ropeAnimationCurve.Evaluate(delta) * waveSize;

                // Always interpolate from grapplePoint to firePoint
                targetPosition = Vector2.Lerp(grapplingGun.grapplePoint, grapplingGun.firePoint.position, delta) + offset;
                currentPosition = Vector2.Lerp(grapplingGun.grapplePoint, targetPosition, ropeLaunchSpeedCurve.Evaluate(moveTime) * ropeLaunchSpeedMultiplayer);

                m_lineRenderer.SetPosition(i, currentPosition);
            }
            else if (newGrapplingGun != null)
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
            newGrapplingGun.DisableGrapple();
        }
    }



    void DrawRopeNoWaves() 
    {
        if (grapplingGun != null)
        {
            m_lineRenderer.positionCount = 2;
            m_lineRenderer.SetPosition(0, grapplingGun.grapplePoint);
            m_lineRenderer.SetPosition(1, grapplingGun.firePoint.position);
        }

        if (newGrapplingGun != null)
        {
            m_lineRenderer.positionCount = 2;
            m_lineRenderer.SetPosition(0, newGrapplingGun.grapplePoint);
            m_lineRenderer.SetPosition(1, newGrapplingGun.firePoint.position);
        }
        
    }

}
