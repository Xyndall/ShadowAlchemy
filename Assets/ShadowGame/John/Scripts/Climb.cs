using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb : MonoBehaviour
{
    public LayerMask Ladder;
    public bool canClimb;
    private void Update()
    {
        detectLadder();
    }
    void detectLadder()
    {
        if(Physics2D.OverlapBox(transform.position, transform.localScale, 0, Ladder))
        {
            canClimb = true;
        }
        else
        {
            canClimb=false;
        }
    }
}
