using UnityEngine;


public class GrappleRope : MonoBehaviour
{
    [Header("General refrences:")]
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
    public int GrappleAmountMissed;

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
            m_lineRenderer.SetPosition(i, newGrapplingGun.firePoint.position);

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
            
                if (m_lineRenderer.GetPosition(percision - 1).x != newGrapplingGun.grapplePoint.x)
                {
                    DrawRopeWaves();
                }
                else
                {
                    straightLine = true;
                }
            
            
        }
        else 
        {
            if (!isGrappling) 
            {
                
                 newGrapplingGun.Grapple();
                
            }
            if (waveSize > 0)
            {
                waveSize -= Time.deltaTime * straightenLineSpeed;
                DrawRopeWaves();
            }
            else 
            {
                waveSize = 0;

                if(newGrapplingGun.validGrapplePoint == false && newGrapplingGun.isSlingshotting == false) RetractRopeWaves();
                else DrawRopeNoWaves();
            }
        }
    }

    void DrawRopeWaves() 
    {
        moveTime += Time.deltaTime;
        for (int i = 0; i < percision; i++)
        {
                float delta = (float)i / ((float)percision - 1f);
                Vector2 offset = Vector2.Perpendicular(newGrapplingGun.DistanceVector).normalized * ropeAnimationCurve.Evaluate(delta) * waveSize;
                Vector2 targetPosition = Vector2.Lerp(newGrapplingGun.firePoint.position, newGrapplingGun.grapplePoint, delta) + offset;
                Vector2 currentPosition = Vector2.Lerp(newGrapplingGun.firePoint.position, targetPosition, ropeLaunchSpeedCurve.Evaluate(moveTime) * ropeLaunchSpeedMultiplayer);

                m_lineRenderer.SetPosition(i, currentPosition);
        }
       
    }

    void RetractRopeWaves()
    {
        GrappleRetracting = true;
        GrappleAmountMissed++;
        SteamAchievements.UnlockAchievement("Ach_GrappleMiss");
        // Slow retraction rate
        moveTime -= Time.deltaTime * (ropeLaunchSpeedMultiplayer / 10f);

        if (m_lineRenderer.positionCount != percision)
        {
            m_lineRenderer.positionCount = percision;
        }

        for (int i = 0; i < percision; i++)
        {
            float delta = (float)i / ((float)percision - 1f);
            Vector2 offset = Vector2.zero;
            Vector2 targetPosition;
            Vector2 currentPosition;

            offset = Vector2.Perpendicular(newGrapplingGun.DistanceVector).normalized * ropeAnimationCurve.Evaluate(delta) * waveSize;

            targetPosition = Vector2.Lerp(newGrapplingGun.grapplePoint, newGrapplingGun.firePoint.position, delta) + offset;
            currentPosition = Vector2.Lerp(newGrapplingGun.firePoint.position, targetPosition, ropeLaunchSpeedCurve.Evaluate(moveTime) * ropeLaunchSpeedMultiplayer);

            m_lineRenderer.SetPosition(i, currentPosition);
        }

        if (moveTime <= 0f)
        {
            moveTime = 0f;
            m_lineRenderer.positionCount = 0; // Clear the line renderer
            newGrapplingGun.DisableGrapple();
        }
    }

    void DrawRopeNoWaves() 
    {
            if (newGrapplingGun.isSlingshotting)
            {
                newGrapplingGun.DisableGrapple();
            }
            m_lineRenderer.positionCount = 2;
            m_lineRenderer.SetPosition(0, newGrapplingGun.grapplePoint);
            m_lineRenderer.SetPosition(1, newGrapplingGun.firePoint.position);

    }

}
