using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] float distanceToCheck = 1f; 
    public bool groundCheck;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, distanceToCheck))
        {
            groundCheck = true;
        }
        else { groundCheck = false; }
    }
}
