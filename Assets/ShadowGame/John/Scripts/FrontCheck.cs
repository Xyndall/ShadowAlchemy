using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCheck : MonoBehaviour
{
    public bool touchingWall;
    public float wallCheckDistance;
    public LayerMask wallMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float lookdirX = Input.GetAxisRaw("Horizontal");
        if (Physics2D.Raycast(transform.position, new Vector2(lookdirX,0), wallCheckDistance,wallMask))
        {
            touchingWall = true;
        }
        else { touchingWall = false; }
    }
}
