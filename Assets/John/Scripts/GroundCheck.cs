using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] Vector2 offset; 
    public bool groundCheck;
    public LayerMask groundMask;
    void Update()
    {
        if (Physics2D.OverlapBox(transform.position, offset,0,groundMask))
        {
            groundCheck = true;
        }
        else { groundCheck = false; }
    }
}
