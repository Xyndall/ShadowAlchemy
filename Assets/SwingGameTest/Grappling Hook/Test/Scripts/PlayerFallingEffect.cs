using UnityEngine;

public class PlayerFallingEffect : MonoBehaviour
{
    public GameObject fallingEffectPrefab; // Assign in Inspector
    public float fallThreshold = -2f; // Speed at which falling effect appears

    private Rigidbody2D rb;
    private GameObject currentEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (NewGrappleTest.instance.grappleRope.isGrappling == false)
        {
            if (rb.velocity.y < fallThreshold)
            {
                if (currentEffect == null && fallingEffectPrefab != null)
                {
                    currentEffect = Instantiate(fallingEffectPrefab, transform.position, Quaternion.identity, transform);
                }
            }
            else
            {
                if (currentEffect != null)
                {
                    Destroy(currentEffect);
                    currentEffect = null;
                }
            }
        }
       
    }
}

