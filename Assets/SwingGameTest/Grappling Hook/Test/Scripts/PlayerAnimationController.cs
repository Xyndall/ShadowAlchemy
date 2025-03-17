using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Rigidbody2D rb;
    private bool isFacingRight; // Reference to the SpriteRenderer. 
    public Animator animator;
    public NewGrappleTest grappleTest;
    public float movementThreshold = .1f; // Minimum velocity to consider as "moving."
    public bool isMoving = false; // Tracks whether the object is moving.
    private void Start()
    {
        isFacingRight = true;
        //grappleTest.ReverseSpin();
    }

    
    public void SetAHoldingButton(bool isTrue)
    {
        animator.SetBool("IsHoldingButton", isTrue);
    }

    public void SetIsGrappling(bool isTrue)
    {
        animator.SetBool("IsGrappling", isTrue);
    }

    private void Update()
    {
        // Ensure Rigidbody2D is available.
        if (rb == null) return;

        // Get the horizontal velocity of the GameObject.
        float horizontalVelocity = rb.velocity.x;

        // Determine if the object is moving.
        isMoving = Mathf.Abs(horizontalVelocity) > movementThreshold;

        // Flip the character based on movement direction if moving.
        if (isMoving)
        {
            if (horizontalVelocity > 0.5f && !isFacingRight)
            {
                Flip(-1);
                //grappleTest.ReverseSpin();
            }
            else if (horizontalVelocity < -0.5f && isFacingRight)
            {
                Flip(1);
                //grappleTest.ReverseSpin();
            }
        }

        animator.SetBool("IsMoving", isMoving);


        if (isMoving)
        {

        }
    }


    private void Flip(float dir)
    {
        isFacingRight = !isFacingRight;

        // Multiply the x scale by -1 to flip the character.
        Vector3 newScale = transform.localScale;
        newScale.x = dir;
        transform.localScale = newScale;
        
    }
}
