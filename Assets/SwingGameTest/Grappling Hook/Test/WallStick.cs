using System.Collections;
using UnityEngine;

public class WallStick : MonoBehaviour
{
    public LayerMask stickableLayer; // The LayerMask for stickable walls
    private Rigidbody2D rb;
    private bool isStuck = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsStickableWall(collision.gameObject) && !isStuck)
        {
            StickToWall();
        }
    }

    bool IsStickableWall(GameObject obj)
    {
        // Check if the collided object is on the stickableLayer
        return (stickableLayer.value & (1 << obj.layer)) > 0;
    }

    void StickToWall()
    {
        isStuck = true;
        rb.velocity = Vector2.zero; // Stop any current movement
        rb.isKinematic = true; // Make the Rigidbody2D kinematic to stick to the wall
    }

    public void UnstickFromWall()
    {
        if (isStuck)
        {
            StartCoroutine(WaitForUnstick());
            rb.isKinematic = false; // Re-enable physics simulation
        }
    }

    IEnumerator WaitForUnstick()
    {
        yield return new WaitForSeconds(.5f);
        isStuck = false;
    }
}
